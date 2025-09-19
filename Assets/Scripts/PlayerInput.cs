using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerAction _inputActions;

    public Action OnLeftMovePressed;
    public Action OnLeftMoveReleased;
    public Action OnRightMovePressed;
    public Action OnRightMoveReleased;
    public Action OnJumpPressed;
    public Action OnJumpReleased;
    public Action OnSlidePressed;
    public Action OnSlideReleased;

    private void Awake()
    {
        _inputActions = new PlayerAction();

        Subscribe();
    }
    private void OnEnable()
    {
        _inputActions.FoxAction.Enable();
    }
    private void OnDisable()
    {
        _inputActions.FoxAction.Disable();
    }
    private void OnDestroy()
    {
        if (_inputActions != null)
        {
            Unsubscribe();
            _inputActions.Dispose();
        }
    }
    private void Subscribe()
    {
        _inputActions.FoxAction.MoveLeft.performed += OnMoveLeftPerformed;
        _inputActions.FoxAction.MoveLeft.canceled += OnMoveLeftCanceled;

        _inputActions.FoxAction.MoveRight.performed += OnMoveRightPerformed;
        _inputActions.FoxAction.MoveRight.canceled += OnMoveRightCanceled;

        _inputActions.FoxAction.Jump.performed += OnJumpPerformed;
        _inputActions.FoxAction.Jump.canceled += OnJumpCanceled;

        _inputActions.FoxAction.Slide.performed += OnSlidePerformed;
        _inputActions.FoxAction.Slide.canceled += OnSlideCanceled;
    }
    private void Unsubscribe()
    {
        _inputActions.FoxAction.MoveLeft.performed -= OnMoveLeftPerformed;
        _inputActions.FoxAction.MoveLeft.canceled -= OnMoveLeftCanceled;

        _inputActions.FoxAction.MoveRight.performed -= OnMoveRightPerformed;
        _inputActions.FoxAction.MoveRight.canceled -= OnMoveRightCanceled;

        _inputActions.FoxAction.Jump.performed -= OnJumpPerformed;
        _inputActions.FoxAction.Jump.canceled -= OnJumpCanceled;

        _inputActions.FoxAction.Slide.performed -= OnSlidePerformed;
        _inputActions.FoxAction.Slide.canceled -= OnSlideCanceled;
    }
    private void OnMoveLeftPerformed(InputAction.CallbackContext ctx)
    {
        OnLeftMovePressed?.Invoke();
    }
    private void OnMoveLeftCanceled(InputAction.CallbackContext ctx)
    {
        OnLeftMoveReleased?.Invoke();
    }
    private void OnMoveRightPerformed(InputAction.CallbackContext ctx)
    {
        OnRightMovePressed?.Invoke();
    }
    private void OnMoveRightCanceled(InputAction.CallbackContext ctx)
    {
        OnRightMoveReleased?.Invoke();
    }
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnJumpPressed?.Invoke();
    }
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        OnJumpReleased?.Invoke();
    }
    private void OnSlidePerformed(InputAction.CallbackContext ctx)
    {
        OnSlidePressed?.Invoke();
    }
    private void OnSlideCanceled(InputAction.CallbackContext ctx)
    {
        OnSlideReleased?.Invoke();
    }
}