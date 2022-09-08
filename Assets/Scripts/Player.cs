using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 50;
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    private float jabValue;
    [SerializeField] private float jabSpeed = 0.6f;
    
    // Health

    public Animator animator;

    // Animation States
    const string STATE_IDLE = "idle";
    const string STATE_JAB = "jab";
    const string STATE_RIGHT = "right";
    const string STATE_DODGE_LEFT = "dodge-left";
    const string STATE_DODGE_RIGHT = "dodge-right";
    const string STATE_BLOCK = "block";
    // Animation Variables
    private bool isJabbing = false;
    private bool isRightHitting = false;
    private bool isDodging = false;
    private bool isBlocking = false;

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

    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !isJabbing && !isDodging && !isRightHitting) {
            isJabbing = true;
            animator.Play(STATE_JAB);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            Health.TakeDamageEnemy(10);
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !isJabbing && !isDodging && !isRightHitting) {
            isRightHitting = true;
            animator.Play(STATE_RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            Health.TakeDamageEnemy(10);
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !isJabbing && !isDodging && !isRightHitting) {
            isBlocking = true;
            animator.Play(STATE_BLOCK);
        } else {
            isBlocking = false;
            animator.Play(STATE_IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !isJabbing && !isDodging && !isRightHitting) {
            isDodging = true;
            animator.Play(STATE_DODGE_RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !isJabbing && !isDodging && !isRightHitting) {
            isDodging = true;
            animator.Play(STATE_DODGE_LEFT);
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
        isDodging = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
