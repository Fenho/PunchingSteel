using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;
    public GameLogic gameLogic;

    // Animation Variables
    [SerializeField] protected bool shouldCueRightHitting = true;
    [SerializeField] protected bool shouldCueJab = true;
    [SerializeField] protected bool shouldTakeTeamHealth = true;
    [SerializeField] protected bool shouldPlayDodgeSound = true;

    [SerializeField] protected float jabSpeed = 0.5f;
    [SerializeField] protected float cueSpeed = 0.3f;

    [SerializeField] public string enemyState = State.IDLE;

    // Music
    public AudioSource audioSource;
    public AudioClip punchSound1;
    public AudioClip punchSound2;
    public AudioClip dodgeSound1;
    public AudioClip dodgeSound2;
    public AudioClip missSound;
    public AudioClip blockSound;

    public float volume = 1.0f;

    // Possible Actions
    // Jab, Right, Block, DodgeLeft, DodgeRight
    protected string action;
    protected string[] actions = new string[] { State.JAB, State.RIGHT, State.BLOCK, State.DODGE_LEFT, State.DODGE_RIGHT};

    // Health
    private int DAMAGE = 10;
    
    public void SetBlocking() {
        enemyState = action = State.BLOCK;
    }

    public void PlayBlockSound() {
        audioSource.PlayOneShot(blockSound, volume);
    }

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    protected void OnJab() {
        enemyState = State.JAB;
        if (shouldCueJab) {
            animator.Play(State.JAB_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        } else {
            animator.Play(State.JAB);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound1, volume);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                }
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        
    }

    protected void OnRight() {
        enemyState = State.RIGHT;
        if (shouldCueRightHitting) {
            animator.Play(State.RIGHT_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        }
        else {
            animator.Play(State.RIGHT);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound2, volume);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                }
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    protected void OnBlock() {
        enemyState = State.BLOCK;
        animator.Play(State.BLOCK);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    protected void OnDodgeLeft() {
        enemyState = State.DODGE_LEFT;
        animator.Play(State.DODGE_LEFT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound2, volume);
            shouldPlayDodgeSound = false;
        }
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    protected void OnDodgeRight() {
        enemyState = State.DODGE_RIGHT;
        animator.Play(State.DODGE_RIGHT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound1, volume);
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

    protected void BackToIdle() {
        animator.Play(State.IDLE);
        action = State.IDLE;
        shouldCueRightHitting = true;
        shouldCueJab = true;
        shouldTakeTeamHealth = true;
        shouldPlayDodgeSound = true;
        enemyState = State.IDLE;
    }

    protected abstract void Choose();

    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    protected void Update() {
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
