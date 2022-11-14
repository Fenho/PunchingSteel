using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RobotTutorial : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction jabAction;
    private InputAction rightAction;
    private InputAction blockAction;
    private InputAction dodgeLeftAction;
    private InputAction dodgeRightAction;
    private GameObject trainerGo;
    private TrainerTutorial trainer;
    
    public TextMeshProUGUI feedbackText;

    public static int n_punches_right = 0;
    public static int n_punches_left = 0;
    public static int n_dodges = 0;
    public static int n_blocks = 0;
    public bool jabActivated = false;
    public bool dodgeActivated = false;
    public bool blockActivated = false;

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
        trainerGo = GameObject.Find("TrainerTutorial");
        if (trainerGo != null) {
            trainer = trainerGo.GetComponent<TrainerTutorial>();
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
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.JAB && jabActivated) {
            playerState = teamState = State.JAB;
            animator.Play(State.JAB);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            
            audioSource.PlayOneShot(punchSound1, volume);
            n_punches_right += 1;
            feedbackText.text = "That's it! Again!";
            n_punches_left += 1;
            if (n_punches_left == 3){
                dodgeActivated = true;
                feedbackText.text = "Let's try dodging";
            }

            
        }
    }

    private void OnRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && trainer != null && trainer.trainerState == State.RIGHT) {
            playerState = teamState = State.RIGHT;
            animator.Play(State.RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
            
            audioSource.PlayOneShot(punchSound2, volume);
            n_punches_right += 1;
            Debug.Log(n_punches_right);
            feedbackText.text = "Great! Again!";
            if (n_punches_right == 3){
                jabActivated = true;
                feedbackText.text = "Now try a left jab!";
            }
        }
    }

    private void OnBlock(InputAction.CallbackContext context) {
        return;
    }

    private void OnDodgeRight(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && dodgeActivated) {
            playerState = teamState = State.DODGE_RIGHT;
            animator.Play(State.DODGE_RIGHT);
            audioSource.PlayOneShot(dodgeSound1, volume);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));

            feedbackText.text = "Great dodge! Again!";
            n_punches_left += 1;
            n_dodges += 1;
            if (n_dodges == 3){
                blockActivated = true;
                feedbackText.text = "Finally, try blocking!";
            }
        }
    }

    private void OnDodgeLeft(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !DoingSomething() && dodgeActivated) {
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
