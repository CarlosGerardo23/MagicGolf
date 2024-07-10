using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BallController : MonoBehaviour
{
    [Header("Drag variables")]
    [SerializeField] private float _dragFactor;
    [SerializeField] private float _reductionDuration;
    [SerializeField] private float _minVelocity;
    private bool _isReducing;

    private float _reductionStartTime;
    private Rigidbody _ballRB;
    private void Awake()
    {
        _ballRB = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Platform"))
        {
            _isReducing = true;
            _reductionStartTime = Time.time;
        }
    }
    void FixedUpdate()
    {
        if (_ballRB.linearVelocity.magnitude <= _minVelocity)
            _ballRB.linearVelocity = Vector3.zero;

        if (_isReducing)
        {
            float elapsedTime = Time.time - _reductionStartTime;
            if (elapsedTime < _reductionDuration)
            {
                float reductionAmount = _dragFactor * Time.deltaTime;
                _ballRB.linearVelocity = Vector3.Lerp(_ballRB.linearVelocity, Vector3.zero, reductionAmount);
                Debug.Log($"drag factor {reductionAmount} {elapsedTime}");
            }
            else
            {
                _isReducing = false;
            }
        }

    }
    public void LaunchBall(Vector3 direction)
    {
        
    }

}
