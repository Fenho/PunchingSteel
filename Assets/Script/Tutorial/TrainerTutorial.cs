using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrainerTutorial : MonoBehaviour
{
    // Inputs
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    private GameObject robotGo;
    private RobotTutorial robot;
    
    [SerializeField] private float jabSpeed = 0.6f;
    
    public Animator animator;
    
    // Animation Variables
    [SerializeField] public string trainerState = State.IDLE;

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
        robotGo = GameObject.Find("RobotTutorial");
        if (robotGo != null) {
            robot = robotGo.GetComponent<RobotTutorial>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private bool DoingSomething() {
        return !trainerState.Equals(State.IDLE);
    }


    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && robot.jabActivated == true) {
            trainerState = State.JAB;
            animator.Play(State.JAB);
            // audioSource.PlayOneShot(punchSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = State.RIGHT;
            animator.Play(State.RIGHT);
            // audioSource.PlayOneShot(punchSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && robot.blockActivated == true) {
            trainerState = State.BLOCK;
            animator.Play(State.BLOCK);
        } else {
            trainerState = State.IDLE;
            animator.Play(State.IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && robot.dodgeActivated == true) {
            trainerState = State.DODGE_RIGHT;
            animator.Play(State.DODGE_RIGHT);
            // audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        return;
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && robot.dodgeActivated == true) {
            trainerState = State.DODGE_LEFT;
            animator.Play(State.DODGE_LEFT);
            // audioSource.PlayOneShot(dodgeSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        return;
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    private void BackToIdle() {
        if (trainerState == State.BLOCK) {
            animator.Play(State.BLOCK);
        } else {
            animator.Play(State.IDLE);
        }
        trainerState = State.IDLE;
    }
}
