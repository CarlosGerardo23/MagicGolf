using System;
using Unity.VisualScripting;
using UnityEngine;

public class StickController : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;
    [Header("Variables")]
    [SerializeField] private float _forceZ;
    [SerializeField] private float _maxForceY;
    [SerializeField] private float _offsetDistance = 0.5f;
    private Vector2 _initialPosition;
    private Vector2 _finalPosition;
    private Vector3 _forceVector;
    private Rigidbody _ballRigidBody;
    private Vector3 _initialPositionObject;

    private void OnEnable()
    {
        _inputReader.onHitBallStarted += StartLaunch;
        _inputReader.onFingerMoved += SetForceLaunch;
        _inputReader.onHitBallEnded += EndLaunch;
    }


    private void OnDisable()
    {
        _inputReader.onFingerMoved -= SetForceLaunch;
        _inputReader.onHitBallStarted -= StartLaunch;
        _inputReader.onHitBallEnded -= EndLaunch;
    }
    private void SetForceLaunch(Vector2 vector)
    {
        _finalPosition = vector;
    }

    private void StartLaunch()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputReader.TouchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Ball"))
            {
                _ballRigidBody = hit.transform.GetComponent<Rigidbody>();
                _ballRigidBody.isKinematic = false;
                _initialPosition = _inputReader.TouchPosition;
                _initialPositionObject = _ballRigidBody.transform.position;
            }
        }
    }

    private void EndLaunch()
    {
        if (_ballRigidBody != null && Vector2.Distance(_finalPosition, _initialPosition) > _offsetDistance)
        {
            float deltaX = (_finalPosition.x - _initialPosition.x) * -1;
            float deltaY = Mathf.Clamp(_initialPosition.y - _finalPosition.y, 0, _maxForceY);
            _forceVector = new Vector3(deltaX, deltaY, _forceZ);
            _ballRigidBody.AddForce(_forceVector, ForceMode.Force);
        }
    }
    public void ResetBall()
    {
        _ballRigidBody.transform.position = _initialPositionObject;
        _ballRigidBody.linearVelocity = Vector3.zero;
        _ballRigidBody.isKinematic = true;
    }

}
