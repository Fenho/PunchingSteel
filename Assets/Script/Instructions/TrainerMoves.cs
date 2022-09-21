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
    
    [SerializeField] private float jabSpeed = 0.6f;
    public Animator animator;

    // Animation States
    const string STATE_IDLE = "idle";
    const string STATE_JAB = "jab";
    const string STATE_RIGHT = "right";
    const string STATE_DODGE_LEFT = "dodge-left";
    const string STATE_DODGE_RIGHT = "dodge-right";
    const string STATE_BLOCK = "block";
    // Animation Variables
    public bool isJabbing = false;
    public bool isRightHitting = false;
    public bool isDodgingLeft = false;
    public bool isDodgingRight = false;
    public bool isBlocking = false;

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
        return isJabbing || isRightHitting || isDodgingLeft || isDodgingRight || isBlocking;
    }


    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            isJabbing = true;
            animator.Play(STATE_JAB);
            /* audioSource.PlayOneShot(punchSound1, volume); */
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            isRightHitting = true;
            animator.Play(STATE_RIGHT);
            // audioSource.PlayOneShot(punchSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            isBlocking = true;
            animator.Play(STATE_BLOCK);
        } else {
            isBlocking = false;
            animator.Play(STATE_IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            isDodgingRight = true;
            animator.Play(STATE_DODGE_RIGHT);
            // audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            isDodgingLeft = true;
            animator.Play(STATE_DODGE_LEFT);
            // audioSource.PlayOneShot(dodgeSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
    
        // Code to execute after the delay
        BackToIdle();
    }

    private void BackToIdle() {
        if (isBlocking) {
            animator.Play(STATE_BLOCK);
        } else {
            animator.Play(STATE_IDLE);
        }
        isJabbing = false;
        isRightHitting = false;
        isDodgingLeft = false;
        isDodgingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
