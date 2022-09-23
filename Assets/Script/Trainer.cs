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
    
    [SerializeField] private float jabSpeed = 0.6f;
    
    private Animator animator;
    
    // Animation Variables
    [SerializeField] public string trainerState = State.IDLE;
    
    // Stamina
    [SerializeField] private StaminaBar stamina;
    public int HIT_STAMINA_PENALTY = 10;
    private int BLOCK_STAMINA_PENALTY = 5;
    // Every second the player will lose stamina
    int duration = 2; 
    float next = 0;

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
        return !trainerState.Equals(State.IDLE);
    }


    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = State.JAB;
            animator.Play(State.JAB);
            stamina.SetStamina(HIT_STAMINA_PENALTY);
            // audioSource.PlayOneShot(punchSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = State.RIGHT;
            animator.Play(State.RIGHT);
            stamina.SetStamina(HIT_STAMINA_PENALTY);
            // audioSource.PlayOneShot(punchSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = State.BLOCK;
            animator.Play(State.BLOCK);
        } else {
            trainerState = State.IDLE;
            animator.Play(State.IDLE);
        }
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            trainerState = State.DODGE_RIGHT;
            animator.Play(State.DODGE_RIGHT);
            // audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        return;
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
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

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= next && trainerState == State.BLOCK) {
            stamina.SetStamina(BLOCK_STAMINA_PENALTY);
            next = Time.time + duration/2; 
        }
    }
}
