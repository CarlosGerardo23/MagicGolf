using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
[CreateAssetMenu(fileName = "InputReaderSO", menuName = "Scriptable Objects/InputReaderSO")]
public class InputReaderSO : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event Action onHitBallStarted;
    public event Action onHitBallEnded;
    public event Action<Vector2> onFingerMoved;
    public event Action onInteractEvent;
    private bool _startTouch = false;

    private InputSystem_Actions _inputActions;
    public Vector2 TouchPosition => EnhancedTouch.Touch.fingers[0].screenPosition;
    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += StartTouch;
        EnhancedTouch.Touch.onFingerMove += MoveFinger;
        EnhancedTouch.Touch.onFingerUp += StopTouch;
        _startTouch = false;
        if (_inputActions == null)
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player.SetCallbacks(this);
        }
    }


    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= StartTouch;
        EnhancedTouch.Touch.onFingerMove -= MoveFinger;
        EnhancedTouch.Touch.onFingerUp -= StopTouch;
        DisablePlayer();
    }
    public void EnablePlayer()
    {
        _inputActions.Player.Enable();
    }
    public void DisablePlayer()
    {
        _inputActions.Player.Disable();
    }
    private void StartTouch(Finger finger)
    {
        _startTouch = true;
        onHitBallStarted?.Invoke();
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
    #region DefaultPlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onInteractEvent?.Invoke();
            Debug.Log("Start touch");

        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }
    #endregion
}
