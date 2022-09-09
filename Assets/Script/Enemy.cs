using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    // Animation States
    const string STATE_IDLE = "idle";
    const string STATE_JAB = "jab";
    const string STATE_RIGHT = "right";
    const string STATE_DODGE_LEFT = "dodge-left";
    const string STATE_DODGE_RIGHT = "dodge-right";
    const string STATE_BLOCK = "block";
    const string STATE_JAB_CUE = "jab-cue";
    const string STATE_RIGHT_CUE = "right-cue";
    // Animation Variables
    [SerializeField] private bool isJabbing = false;
    [SerializeField] private bool isRightHitting = false;
    [SerializeField] private bool isDodgingLeft = false;
    [SerializeField] private bool isDodgingRight = false;
    [SerializeField] private bool isBlocking = false;
    [SerializeField] private bool shouldCueRightHitting = true;
    [SerializeField] private bool shouldCueJab = true;

    [SerializeField] private float jabSpeed = 0.5f;
    [SerializeField] private float cueSpeed = 0.3f;

    // Possible Actions
    // Jab, Right, Block, DodgeLeft, DodgeRight
    private string action;
    private string[] actions = new string[] { STATE_JAB, STATE_RIGHT, STATE_BLOCK, STATE_DODGE_LEFT, STATE_DODGE_RIGHT };
    private float[] probs = {2, 2, 2, 2, 2};
    
    void Choose() {
        action = actions[Random.Range(0, actions.Length)];
    }


    private void OnJab() {
        isJabbing = true;
        if (shouldCueJab) {
            animator.Play(STATE_JAB_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        } else {
            animator.Play(STATE_JAB);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
        
    }

    private void OnRight() {
        isRightHitting = true;
        if (shouldCueRightHitting) {
            animator.Play(STATE_RIGHT_CUE);
            StartCoroutine(DisableCues(cueSpeed));
        }
        else {
            animator.Play(STATE_RIGHT);
            StartCoroutine(LetAnimationRunForTime(jabSpeed));
        }
    }

    private void OnBlock() {
        isBlocking = true;
        animator.Play(STATE_BLOCK);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    private void OnDodgeRight() {
        isDodgingRight = true;
        animator.Play(STATE_DODGE_RIGHT);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    void OnDodgeLeft() {
        isDodgingLeft = true;
        animator.Play(STATE_DODGE_LEFT);
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

        // Code to execute after the delay
        shouldCueRightHitting = false;
        shouldCueJab = false;
    }

    private void BackToIdle() {
        animator.Play(STATE_IDLE);
        action = STATE_IDLE;
        isJabbing = false;
        isRightHitting = false;
        isDodgingLeft = false;
        isDodgingRight = false;
        shouldCueRightHitting = true;
        shouldCueJab = true;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Choose", 2.0f, 3f);
        // Invoke("Choose", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // action = actions[Choose(probs)];
        if (action == STATE_JAB) {
            OnJab();
        }
        if (action == STATE_RIGHT) {
            OnRight();
        }
        if (action == STATE_BLOCK) {
            OnBlock();
        }
        if (action == STATE_DODGE_LEFT) {
            OnDodgeLeft();
        }
        if (action == STATE_DODGE_RIGHT) {
            OnDodgeRight();
        }
    }
}