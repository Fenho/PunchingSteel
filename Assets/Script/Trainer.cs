using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trainer : MonoBehaviour
{
    // Music
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public float volume=1.0f;

    // Inputs
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    
    [SerializeField] private float jabTime = 0.3f;
    
    private Animator animator;
    
    // Animation Variables
    public string trainerState = RobotState.IDLE;
    private bool shouldPlayWinLoseAfterGameOver = true;
    
    // Stamina
    [SerializeField] private StaminaBar stamina;
    public float JAB_STAMINA_PENALTY = 5f;
    public float RIGHT_STAMINA_PENALTY = 10f;
    private float BLOCK_STAMINA_PENALTY = 20f;
    private float BLOCK_STAMINA_BASE_PENALTY = 1f;
    private bool justPressedBlock = false;

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

    private bool isInPauseMenu()
    {
        return StaticVars.isInPauseMenu;
    }

    private bool IsGameOver() {
        return StaticVars.gameOver;
    }

    

    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && !IsGameOver() && !isInPauseMenu()) {
            trainerState = RobotState.JAB;
            animator.Play(RobotState.JAB);
            stamina.DecreaseStaminaBy(JAB_STAMINA_PENALTY);
            // audioSource.PlayOneShot(punchSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && !IsGameOver() && !isInPauseMenu()) {
            trainerState = RobotState.RIGHT;
            animator.Play(RobotState.RIGHT);
            stamina.DecreaseStaminaBy(RIGHT_STAMINA_PENALTY);
            // audioSource.PlayOneShot(punchSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && !IsGameOver() && !isInPauseMenu()) {
            trainerState = RobotState.BLOCK;
            animator.Play(RobotState.BLOCK);
            justPressedBlock = true;
        } else {
            justPressedBlock = false;
            trainerState = RobotState.IDLE;
            animator.Play(RobotState.IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && !IsGameOver() && !isInPauseMenu()) {
            trainerState = RobotState.DODGE_RIGHT;
            animator.Play(RobotState.DODGE_RIGHT);
            // audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
        return;
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && !IsGameOver() && !isInPauseMenu()) {
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

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver()) {
            if (shouldPlayWinLoseAfterGameOver) {
                if (StaticVars.win) {
                    animator.Play(RobotState.WIN);
                } else {
                    animator.Play(RobotState.LOSE);
                }
                shouldPlayWinLoseAfterGameOver = false;
            }
            return;
        }
        if (trainerState == RobotState.BLOCK) {
            if (justPressedBlock) {
                stamina.DecreaseStaminaBy(BLOCK_STAMINA_BASE_PENALTY);
                justPressedBlock = false;
            }
            stamina.DecreaseStaminaBy(BLOCK_STAMINA_PENALTY * Time.deltaTime);
        }
    }
}
