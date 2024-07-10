using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
[CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/InputReaderSO")]
public class InputReaderSO : ScriptableObject
{
    public event Action onHitBallStarted;
    public event Action onHitBallEnded;
    public event Action<Vector2> onFingerMoved;
    private bool _startTouch = false;

    public Vector2 TouchPosition => EnhancedTouch.Touch.fingers[0].screenPosition;
    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += StartTouch;
        EnhancedTouch.Touch.onFingerMove += MoveFinger;
        EnhancedTouch.Touch.onFingerUp += StopTouch;
        _startTouch = false;
    }


    private void OnDisable()
    {
        // EnhancedTouch.TouchSimulation.Disable();
        // EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= StartTouch;
        EnhancedTouch.Touch.onFingerMove -= MoveFinger;
        EnhancedTouch.Touch.onFingerUp -= StopTouch;
    }
    private void StartTouch(Finger finger)
    {
        _startTouch = true;
        onHitBallStarted?.Invoke();
        //  Debug.Log("Start touch");
    }
    private void MoveFinger(Finger finger)
    {
        if (!_startTouch) return;
        onFingerMoved?.Invoke(finger.screenPosition);
        //  Debug.Log($"finger position {finger.screenPosition}");
    }

    private void StopTouch(Finger finger)
    {
        _startTouch = false;
        onHitBallEnded?.Invoke();
        //Debug.Log("End touch");
    }
}
