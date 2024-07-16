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
    public event Action onInteractEvent;
    private bool _startTouch = false;

    private InputSystem_Actions _inputActions;
    public Vector2 TouchPosition => Mouse.current.position.value;
    private void OnEnable()
    {

        _startTouch = false;
        if (_inputActions == null)
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player.SetCallbacks(this);
        }
    }
    private void OnDisable()
    {
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
    private void StartTouch()
    {
        _startTouch = true;
        onHitBallStarted?.Invoke();
    }


    private void StopTouch()
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
        if (context.phase == InputActionPhase.Performed)
            StartTouch();
        if (context.phase == InputActionPhase.Canceled)
            StopTouch();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }
    #endregion
}
