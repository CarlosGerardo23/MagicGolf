using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.Netcode;

using UnityEngine;
[RequireComponent(typeof(Collider))]
public class BallController : NetworkBehaviour
{
    #region Editor-assigned Variables
    [SerializeField] private InputReaderSO _inputReader;
    [Header("Launch variables")]
    [Tooltip("The constant force in z value whne launhc")]
    [SerializeField] private float _forceZ;
    [Tooltip("Cap force on Y axis for launch")]
    [SerializeField] private float _maxForceY;
    [Tooltip("The minimum required input distance to launch")]
    [SerializeField] private float _offsetDistance = 0.5f;
    [Tooltip("The maximun time on launching mode before reseting the ball")]
    [SerializeField] private float _maxtimeOnLaunch = 5;
    [Tooltip("The layer mask to interact with the ball")]
    [SerializeField] private LayerMask _layerMask;
    [Header("Drag variables")]
    [Tooltip("When touching a platform the drag factor to reduce ball velocity")]
    [SerializeField] private float _dragFactor;
    [Tooltip("The duration of teh drag factor")]

    [SerializeField] private float _reductionDuration;
    [Tooltip("If the ball reach this velocity or less it will stop")]
    [SerializeField] private float _minVelocityToStop;
    #endregion
    #region Private Variables
    private bool _isBallSelected;
    private bool _isBallLaunched;
    private bool _isReducing;
    private float _reductionStartTime;
    private Rigidbody _ballRB;
    private Vector2 _initialPosition;
    private Vector2 _finalPosition;
    private Vector3 _forceVector;
    private Vector3 _initialPositionObject;
    private CinemachineCamera _cinemchineCamera;
    private float _currentTimeLaunhing;
    private PlayerData _playerData;
    private SpawnPoints _spawnPoints;
    #endregion
    #region Delegates
    public event Action onBallLaunch;
    public event Action onBallStopped;
    #endregion
    #region Unity Functions
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        StartCoroutine(SetCamera());
        _ballRB = GetComponent<Rigidbody>();
        _ballRB.isKinematic = true;
        _isBallSelected = false;
        _inputReader.onHitBallStarted += StartLaunch;
        _inputReader.onHitBallEnded += EndLaunch;
        onBallLaunch += BallLaunched;
        onBallStopped += BallStopped;
        transform.position = new Vector3(0, 0.5f, 0);
        _playerData = GetComponent<PlayerData>();
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.onHitBallStarted -= StartLaunch;
        _inputReader.onHitBallEnded -= EndLaunch;
        onBallLaunch -= BallLaunched;
        onBallStopped -= BallStopped;
    }
    private IEnumerator SetCamera()
    {
        yield return new WaitForSeconds(3f);
        _spawnPoints = FindFirstObjectByType<SpawnPoints>();

        SetBallPosition();
        _cinemchineCamera = FindFirstObjectByType<CinemachineCamera>();
        FindFirstObjectByType<UIJoinCode>().HideLoadingCanvas();
        _cinemchineCamera.Follow = transform;
        _cinemchineCamera.LookAt = transform;
        _ballRB.isKinematic = false;


    }
    void OnTriggerEnter(Collider collider)
    {
        if (!IsOwner) return;
        if (collider.transform.CompareTag("Gem"))
        {
            if (collider.TryGetComponent(out GemController gem))
            {
                int value = gem.Collect();
                if (!IsServer) return;
                _playerData.AddScore(value);
            }
        }
        if (collider.transform.CompareTag("Platform"))
        {
            _isReducing = true;
            _reductionStartTime = Time.time;
        }
    }
    private void Update()
    {
        if(_isBallSelected&&!_isBallLaunched)
        {
            SetBallPosition();
        }
        if (_isBallLaunched)
        {
            _currentTimeLaunhing += Time.deltaTime;
            if (_currentTimeLaunhing >= _maxtimeOnLaunch)
                ResetBall();
        }
    }
    void FixedUpdate()
    {
        if (!IsOwner) return;
        if (!_isBallLaunched) return;
        if (!_isReducing) return;
        //        Debug.Log($"{_ballRB.linearVelocity.magnitude}");
        if (TryStopBall())
            onBallStopped?.Invoke();
        // else
        //     ReduceVelocity();
    }
    #endregion
    #region Launch Logic Fuctions
    private void StartLaunch()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputReader.TouchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _layerMask))
        {
            if (hit.transform.gameObject == gameObject)
            {
                _isBallSelected = true;
                _ballRB.isKinematic = false;
                _initialPosition = _inputReader.TouchPosition;
                _initialPositionObject = _ballRB.transform.position;
            }
        }
    }
    private void SetForceLaunch(Vector2 vector)
    {
        _finalPosition = vector;
    }

    private void EndLaunch()
    {
        if (_isBallSelected && Vector2.Distance(_finalPosition, _initialPosition) > _offsetDistance)
        {
            _isBallSelected = false;
            float deltaX = (_finalPosition.x - _initialPosition.x) * -1;
            float deltaY = Mathf.Clamp(_initialPosition.y - _finalPosition.y, 0, _maxForceY);
            _forceVector = new Vector3(deltaX, deltaY, _forceZ);
            _ballRB.AddForce(_forceVector, ForceMode.Force);
            onBallLaunch?.Invoke();
        }
    }

    private void BallLaunched()
    {
        _isBallLaunched = true;
    }
    #endregion
    #region Reset

    private void ResetBall()
    {
        _ballRB.transform.position = _initialPositionObject;
        _ballRB.linearVelocity = Vector3.zero;
        _ballRB.isKinematic = true;
        ResetValues();

    }
    private void SetBallPosition()
    {
        _ballRB.transform.position = _spawnPoints.GetPosition();
        _ballRB.linearVelocity = Vector3.zero;
        ResetValues();
    }
    private void ResetValues()
    {
        _isBallLaunched = false;
        _isReducing = false;
        _isBallSelected = false;
        _currentTimeLaunhing = 0;
    }
    #endregion
    #region Drag Ball logic Fuctions
    private bool TryStopBall()
    {
        if (_ballRB.linearVelocity.magnitude <= _minVelocityToStop)
        {
            _ballRB.linearVelocity = Vector3.zero;
            return true;
        }
        return false;
    }

    private void ReduceVelocity()
    {

        float elapsedTime = Time.time - _reductionStartTime;
        if (elapsedTime < _reductionDuration)
        {
            float reductionAmount = _dragFactor * Time.deltaTime;
            _ballRB.linearVelocity = Vector3.Lerp(_ballRB.linearVelocity, Vector3.zero, reductionAmount);
        }

    }

    private void BallStopped()
    {
        ResetValues();
    }
    #endregion
}
