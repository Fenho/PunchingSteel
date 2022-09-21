using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public GameLogic gameLogic;

    // Animation Variables
    [SerializeField] private bool shouldCueRightHitting = true;
    [SerializeField] private bool shouldCueJab = true;
    [SerializeField] private bool shouldTakeTeamHealth = true;
    [SerializeField] private bool shouldPlayDodgeSound = true;

    [SerializeField] private float jabSpeed = 0.5f;
    [SerializeField] private float cueSpeed = 0.3f;

    [SerializeField] public string enemyState = State.IDLE;

    // Music
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public float volume = 1.0f;

    // Possible Actions
    // Jab, Right, Block, DodgeLeft, DodgeRight
    private string action;
    private string[] actions = new string[] { State.JAB, State.RIGHT, State.BLOCK, State.DODGE_LEFT, State.DODGE_RIGHT };
    private float[] probs = {2, 2, 2, 2, 2};
    

    private void Awake() {
        animator = GetComponent<Animator>();
    }


    void Choose() {
        action = actions[Random.Range(0, actions.Length)];
    }


    private void OnJab() {
        enemyState = State.JAB;
        if (shouldCueJab) {
            animator.Play(State.JAB_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        } else {
            animator.Play(State.JAB);
            if (shouldTakeTeamHealth) {
                gameLogic.TakeDamageTeam(10);
                audioSource.PlayOneShot(punchSound1, volume);
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        
    }

    private void OnRight() {
        enemyState = State.RIGHT;
        if (shouldCueRightHitting) {
            animator.Play(State.RIGHT_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        }
        else {
            animator.Play(State.RIGHT);
            if (shouldTakeTeamHealth) {
                gameLogic.TakeDamageTeam(10);
                audioSource.PlayOneShot(punchSound2, volume);
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnBlock() {
        enemyState = State.BLOCK;
        animator.Play(State.BLOCK);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    private void OnDodgeRight() {
        enemyState = State.DODGE_RIGHT;
        animator.Play(State.DODGE_RIGHT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound1, volume);
            shouldPlayDodgeSound = false;
        }
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    void OnDodgeLeft() {
        enemyState = State.DODGE_LEFT;
        animator.Play(State.DODGE_LEFT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound2, volume);
            shouldPlayDodgeSound = false;
        }
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        BackToIdle();
    }

    IEnumerator DisableCues(float time)
    {
        yield return new WaitForSeconds(time);
        shouldCueRightHitting = false;
        shouldCueJab = false;
    }

    private void BackToIdle() {
        animator.Play(State.IDLE);
        action = State.IDLE;
        shouldCueRightHitting = true;
        shouldCueJab = true;
        shouldTakeTeamHealth = true;
        shouldPlayDodgeSound = true;
        enemyState = State.IDLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Choose", 2.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // action = actions[Choose(probs)];
        if (action == State.JAB) {
            OnJab();
        }
        if (action == State.RIGHT) {
            OnRight();
        }
        if (action == State.BLOCK) {
            OnBlock();
        }
        if (action == State.DODGE_LEFT) {
            OnDodgeLeft();
        }
        if (action == State.DODGE_RIGHT) {
            OnDodgeRight();
        }
    }
}
