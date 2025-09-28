using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float laneWidth;
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [Header("Slide Settings")]
    [SerializeField] private float slideHeight;
    [SerializeField] private float slideDuration;
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody _playerRigidbody;
    private BoxCollider _playerCollider;
    private PlayerInput _playerInput;

    // Lane
    private LanePosition currentLane = LanePosition.Center;

    // Bool Variables
    private bool canJump = true;
    private bool isSliding = false;

    // Events
    public static event Action OnMovingLeft;
    public static event Action OnMovingRight;
    public static event Action OnJumpStarted;
    public static event Action OnSlideStarted;
    public static event Action OnGroundLanded;
    public static event Action OnGameLost;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<BoxCollider>();
        _playerInput = GetComponent<PlayerInput>();

        _playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void OnEnable()
    {
        _playerInput.OnLeftMovePressed += HandleLeftMove;
        _playerInput.OnRightMovePressed += HandleRightMove;

        _playerInput.OnJumpPressed += HandleJump;

        _playerInput.OnSlidePressed += HandleSlide;
    }
    private void OnDisable()
    {
        _playerInput.OnLeftMovePressed -= HandleLeftMove;
        _playerInput.OnRightMovePressed -= HandleRightMove;

        _playerInput.OnJumpPressed -= HandleJump;

        _playerInput.OnSlidePressed -= HandleSlide;
    }

    #region Movement
    private void HandleLeftMove()
    {
        if (currentLane != LanePosition.Left)
        {
            OnMovingLeft?.Invoke();
            currentLane--;
            MoveTo(currentLane);
        }
    }
    private void HandleRightMove()
    {
        if (currentLane != LanePosition.Right)
        {
            OnMovingRight?.Invoke();
            currentLane++;
            MoveTo(currentLane);
        }
    }
    private void MoveTo(LanePosition targetLane)
    {
        StartCoroutine(DelayedMovement(targetLane));
    }
    private IEnumerator DelayedMovement(LanePosition targetLane)
    {
        yield return null;

        float target = (int)targetLane * laneWidth;
        Vector3 targetPosition = new Vector3(target, transform.position.y, transform.position.z);

        _playerRigidbody.MovePosition(targetPosition);
    }
    #endregion

    #region Jump
    private void HandleJump()
    {
        if (IsGrounded() && canJump && !isSliding)
        {
            _playerRigidbody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            OnJumpStarted?.Invoke();
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }
    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }
    #endregion

    #region Slide
    private void HandleSlide()
    {
        if (IsGrounded() && !isSliding)
        {
            StartCoroutine(StartSlide());
        }
    }
    private IEnumerator StartSlide()
    {
        isSliding = true;

        OnSlideStarted?.Invoke();

        float originalHeight = _playerCollider.size.y;
        Vector3 originalSize = _playerCollider.size;
        Vector3 originalCenter = _playerCollider.center;

        _playerCollider.size = new Vector3(_playerCollider.size.x, slideHeight, _playerCollider.size.z);

        float offsetChange = (originalHeight - slideHeight) / 2f;
        _playerCollider.center = new Vector3(originalCenter.x, originalCenter.y - offsetChange, originalCenter.z);

        yield return new WaitForSeconds(slideDuration);

        _playerCollider.size = originalSize;
        _playerCollider.center = originalCenter;

        isSliding = false;
    }
    #endregion

    #region Public Helpers
    public bool CanJump => canJump;
    public bool GroundCheck()
    {
        return IsGrounded();
    }
    #endregion
}