using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotMoves : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    private GameObject trainerGo;
    private TrainerMoves trainer;

    // Music
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public AudioClip missSound;

    public float volume = 1.0f;

    [SerializeField] private float jabSpeed = 0.5f;

    [SerializeField] public string teamState = State.IDLE;

    public Animator animator;

    // Animation Variables
    [SerializeField] public string playerState = State.IDLE;

    private void OnDisable() {
        jabAction.Disable();
        rightAction.Disable();
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        trainerGo = GameObject.Find("TrainerMoves");
        if (trainerGo != null) {
            trainer = trainerGo.GetComponent<TrainerMoves>();
        }
        jabAction = playerInput.actions["Jab"];
        jabAction.performed += OnJab;
        rightAction = playerInput.actions["RightDirect"];
        rightAction.performed += OnRight;
        // blockAction = playerInput.actions["Block"];
        // blockAction.performed += OnBlock;
        // blockAction.canceled += OnBlock;
        dodgeRightAction = playerInput.actions["DodgeRight"];
        dodgeRightAction.performed += OnDodgeRight;
        dodgeLeftAction = playerInput.actions["DodgeLeft"];
        dodgeLeftAction.performed += OnDodgeLeft;
    }


    private bool DoingSomething() {
        return !playerState.Equals(State.IDLE);
    }

    // private bool isTeamDoingSomething() {
    //     return !teamState.Equals(State.IDLE);
    // }

    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.JAB) {
            playerState = teamState = State.JAB;
            animator.Play(State.JAB);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            
            audioSource.PlayOneShot(punchSound1, volume);
            
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.RIGHT) {
            playerState = teamState = State.RIGHT;
            animator.Play(State.RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            
            audioSource.PlayOneShot(punchSound2, volume);
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        return;
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            playerState = teamState = State.DODGE_RIGHT;
            animator.Play(State.DODGE_RIGHT);
            audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            playerState = teamState = State.DODGE_LEFT;
            animator.Play(State.DODGE_LEFT);
            audioSource.PlayOneShot(dodgeSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    private void BackToIdle() {
        if (playerState == State.BLOCK) {
            animator.Play(State.BLOCK);
        } else {
            animator.Play(State.IDLE);
        }
        playerState = teamState = State.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (trainer.trainerState == State.BLOCK) {
            playerState = teamState = State.BLOCK;
            animator.Play(State.BLOCK);
        }
        if (trainer.trainerState != State.BLOCK && !(DoingSomething() && !playerState.Equals(State.BLOCK))) {
            playerState = teamState = State.IDLE;
            animator.Play(State.IDLE);
        }
    }
}
