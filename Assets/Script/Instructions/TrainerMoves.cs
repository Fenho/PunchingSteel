using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrainerMoves : MonoBehaviour
{
    // Inputs
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    
    [SerializeField] private float jabTime = 0.6f;
    
    public Animator animator;
    
    // Animation Variables
    [SerializeField] public string trainerState = RobotState.IDLE;

    private void OnDisable() {
        jabAction.Disable();
        rightAction.Disable();
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        jabAction = playerInput.actions["Jab"];
        jabAction.performed += OnJab;
        rightAction = playerInput.actions["RightDirect"];
        rightAction.performed += OnRight;
        blockAction = playerInput.actions["Block"];
        blockAction.performed += OnBlock;
        blockAction.canceled += OnBlock;
        dodgeRightAction = playerInput.actions["DodgeRight"];
        dodgeRightAction.performed += OnDodgeRight;
        dodgeLeftAction = playerInput.actions["DodgeLeft"];
        dodgeLeftAction.performed += OnDodgeLeft;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private bool DoingSomething() {
        return !trainerState.Equals(RobotState.IDLE);
    }


    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = RobotState.JAB;
            animator.Play(RobotState.JAB);
            // audioSource.PlayOneShot(punchSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = RobotState.RIGHT;
            animator.Play(RobotState.RIGHT);
            // audioSource.PlayOneShot(punchSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = RobotState.BLOCK;
            animator.Play(RobotState.BLOCK);
        } else {
            trainerState = RobotState.IDLE;
            animator.Play(RobotState.IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = RobotState.DODGE_RIGHT;
            animator.Play(RobotState.DODGE_RIGHT);
            // audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
        return;
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = RobotState.DODGE_LEFT;
            animator.Play(RobotState.DODGE_LEFT);
            // audioSource.PlayOneShot(dodgeSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
        return;
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    private void BackToIdle() {
        if (trainerState == RobotState.BLOCK) {
            animator.Play(RobotState.BLOCK);
        } else {
            animator.Play(RobotState.IDLE);
        }
        trainerState = RobotState.IDLE;
    }
}
