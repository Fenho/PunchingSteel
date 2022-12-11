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

    [SerializeField] private float jabTime = 0.5f;

    [SerializeField] public string teamState = RobotState.IDLE;

    public Animator animator;

    // Animation Variables
    [SerializeField] public string playerState = RobotState.IDLE;

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
        return !playerState.Equals(RobotState.IDLE);
    }

    // private bool isTeamDoingSomething() {
    //     return !teamState.Equals(RobotState.IDLE);
    // }

    private void OnJab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.JAB) {
            playerState = teamState = RobotState.JAB;
            animator.Play(RobotState.JAB);
            StartCoroutine(LetAnimationRunForTime(jabTime));
            
            audioSource.PlayOneShot(punchSound1, volume);
            
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == RobotState.RIGHT) {
            playerState = teamState = RobotState.RIGHT;
            animator.Play(RobotState.RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabTime));
            
            audioSource.PlayOneShot(punchSound2, volume);
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        return;
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            playerState = teamState = RobotState.DODGE_RIGHT;
            animator.Play(RobotState.DODGE_RIGHT);
            audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething()) {
            playerState = teamState = RobotState.DODGE_LEFT;
            animator.Play(RobotState.DODGE_LEFT);
            audioSource.PlayOneShot(dodgeSound2, volume);
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);
        BackToIdle();
    }

    private void BackToIdle() {
        if (playerState == RobotState.BLOCK) {
            animator.Play(RobotState.BLOCK);
        } else {
            animator.Play(RobotState.IDLE);
        }
        playerState = teamState = RobotState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (trainer.trainerState == RobotState.BLOCK) {
            playerState = teamState = RobotState.BLOCK;
            animator.Play(RobotState.BLOCK);
        }
        if (trainer.trainerState != RobotState.BLOCK && !(DoingSomething() && !playerState.Equals(RobotState.BLOCK))) {
            playerState = teamState = RobotState.IDLE;
            animator.Play(RobotState.IDLE);
        }
    }
}
