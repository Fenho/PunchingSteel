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
    private bool isJabbing = false;
    private bool isRightHitting = false;
    private bool isDodgingLeft = false;
    private bool isDodgingRight = false;
    private bool isBlocking = false;

    [SerializeField] private float jabSpeed = 0.5f;

    // Possible Actions
    // Jab, Right, Block, DodgeLeft, DodgeRight
    private string action;
    private string[] actions = new string[] { STATE_JAB, STATE_RIGHT, STATE_BLOCK, STATE_DODGE_LEFT, STATE_DODGE_RIGHT };
    private float[] probs = {2, 2, 2, 2, 2};

    // void Choose() {

    //     float total = 0;

    //     foreach (float elem in probs) {
    //         total += elem;
    //     }

    //     float randomPoint = Random.value * total;

    //     for (int i= 0; i < probs.Length; i++) {
    //         if (randomPoint < probs[i]) {
    //             action = actions[i];
    //             return;
    //         }
    //         else {
    //             randomPoint -= probs[i];
    //         }
    //     }
    //     return;
    //     // return probs.Length - 1;
    // }

    void Choose() {
        action = actions[Random.Range(0, actions.Length)];
    }

    // IEnumerator CueJab(float time)
    // {
    //     yield return new WaitForSeconds(time);
    
    //     // Code to execute after the delay
    //     animator.Play(STATE_JAB);
    //     StartCoroutine(LetAnimationRunForTime(jabSpeed));
    // }

    // IEnumerator CueRight(float time)
    // {
    //     yield return new WaitForSeconds(time);
    
    //     // Code to execute after the delay
    //     animator.Play(STATE_RIGHT);
    //     StartCoroutine(LetAnimationRunForTime(jabSpeed));
    // }

    void CueJab()
    {
        animator.Play(STATE_JAB);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    void CueRight()
    {
        animator.Play(STATE_RIGHT);
        StartCoroutine(LetAnimationRunForTime(jabSpeed));
    }

    private void OnJab() {
        isJabbing = true;
        animator.Play(STATE_JAB_CUE);
        Invoke("CueJab", jabSpeed);
    }

    private void OnRight() {
        isRightHitting = true;
        animator.Play(STATE_RIGHT_CUE);
        Invoke("CueRight", jabSpeed);
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

    private void BackToIdle() {
        animator.Play(STATE_IDLE);
        action = STATE_IDLE;
        isJabbing = false;
        isRightHitting = false;
        isDodgingLeft = false;
        isDodgingRight = false;
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
