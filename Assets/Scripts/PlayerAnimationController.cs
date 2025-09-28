using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _playerAnimator;
    private PlayerMovement _playerMovement;

    private int isMovingLeftHash;
    private int isMovingRightHash;
    private int isJumpingHash;
    private int isSlidingHash;
    private int isGroundedHash;
    private int isLosingHash;

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();

        isMovingLeftHash = Animator.StringToHash("IsMovingLeft");
        isMovingRightHash = Animator.StringToHash("IsMovingRight");
        isJumpingHash = Animator.StringToHash("IsJumping");
        isSlidingHash = Animator.StringToHash("IsSliding");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        isLosingHash = Animator.StringToHash("IsLosing");

        _playerAnimator.SetBool(isGroundedHash, true);
    }
    private void Update()
    {
        bool isCurrentlyGrounded = _playerMovement.GroundCheck();
        _playerAnimator.SetBool(isGroundedHash, isCurrentlyGrounded);

    }
    private void OnEnable()
    {
        PlayerMovement.OnLaneChanged += HandleLaneChange;
        PlayerMovement.OnJumpStarted += HandleJumpStart;
        PlayerMovement.OnSlideStarted += HandleSlideStart;
    }
    private void OnDisable()
    {
        PlayerMovement.OnLaneChanged -= HandleLaneChange;
        PlayerMovement.OnJumpStarted -= HandleJumpStart;
        PlayerMovement.OnSlideStarted -= HandleSlideStart;
    }
    private void HandleSlideStart()
    {
        _playerAnimator.SetBool(isSlidingHash, true);
        StartCoroutine(ResetBoolAfterDelay(isSlidingHash, 0.5f));
    }

    private void HandleJumpStart()
    {
        _playerAnimator.SetBool(isJumpingHash, true);
        StartCoroutine(ResetBoolAfterDelay(isJumpingHash, 0.5f));
    }

    private void HandleLaneChange(LanePosition position)
    {
        if (position == LanePosition.Left)
        {
            _playerAnimator.SetBool(isMovingLeftHash, true);
            StartCoroutine(ResetBoolAfterDelay(isMovingLeftHash, 0f));
        }
        else if (position == LanePosition.Right)
        {
            _playerAnimator.SetBool(isMovingRightHash, true);
            StartCoroutine(ResetBoolAfterDelay(isMovingRightHash, 0f));
        }
    }

    private IEnumerator ResetBoolAfterDelay(int hash, float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerAnimator.SetBool(hash, false);
    }
}