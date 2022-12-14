using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameLogic gameLogic;

    // Animation Variables
    [SerializeField] protected bool shouldCueRightHitting = true;
    [SerializeField] protected bool shouldCueJab = true;
    [SerializeField] protected bool shouldTakeTeamHealth = true;
    [SerializeField] protected bool shouldPlayDodgeSound = true;
    [SerializeField] protected bool shouldPlayWinLoseAfterGameOver = true;

    [SerializeField] protected float jabTime = 0.3f;
    [SerializeField] protected float cueTime = 0.3f;

    [SerializeField] public string enemyState = EnemyState.IDLE;

    [SerializeField] protected SimpleFlash flashEffect;

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
    protected string[] actions = new string[] { EnemyState.JAB, EnemyState.RIGHT, EnemyState.BLOCK, EnemyState.DODGE_LEFT, EnemyState.DODGE_RIGHT};

    // Health
    protected int DAMAGE = 6;
    
    public void SetBlocking() {
        enemyState = action = EnemyState.BLOCK;
    }

    public void PlayBlockSound() {
        audioSource.PlayOneShot(blockSound, volume);
    }

    protected void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected void OnJab() {
        enemyState = EnemyState.JAB;
        if (shouldCueJab) {
            animator.Play(EnemyState.JAB_CUE);
            StartCoroutine(PlayCue(cueTime));
        } else {
            animator.Play(EnemyState.JAB);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    StaticVars.addPointsByType("LeftJabEnemy");
                    audioSource.PlayOneShot(punchSound1, volume);
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPointsByType("missEnemy");
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPointsByType("blockEnemy");
                }
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
        
    }

    protected void OnRight() {
        enemyState = EnemyState.RIGHT;
        if (shouldCueRightHitting) {
            animator.Play(EnemyState.RIGHT_CUE);
            StartCoroutine(PlayCue(cueTime));
        }
        else {
            animator.Play(EnemyState.RIGHT);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    audioSource.PlayOneShot(punchSound2, volume);
                    StaticVars.addPointsByType("RightJabEnemy");
                } else if (punchResult == GameLogic.PunchResult.MISS) {
                    audioSource.PlayOneShot(missSound, volume);
                    StaticVars.addPointsByType("missEnemy");
                } else if (punchResult == GameLogic.PunchResult.BLOCK) {
                    audioSource.PlayOneShot(blockSound, volume);
                    StaticVars.addPointsByType("blockEnemy");
                }
                shouldTakeTeamHealth = false;
            }
            StartCoroutine(LetAnimationRunForTime(jabTime));
        }
    }

    protected void OnBlock() {
        enemyState = EnemyState.BLOCK;
        animator.Play(EnemyState.BLOCK);
        StartCoroutine(LetAnimationRunForTime(jabTime));
    }

    protected void OnDodgeLeft() {
        enemyState = EnemyState.DODGE_LEFT;
        animator.Play(EnemyState.DODGE_LEFT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound2, volume);
            shouldPlayDodgeSound = false;
        }
        StartCoroutine(LetAnimationRunForTime(jabTime));
    }

    protected void OnDodgeRight() {
        enemyState = EnemyState.DODGE_RIGHT;
        animator.Play(EnemyState.DODGE_RIGHT);
        if (shouldPlayDodgeSound) {
            audioSource.PlayOneShot(dodgeSound1, volume);
            shouldPlayDodgeSound = false;
        }
        StartCoroutine(LetAnimationRunForTime(jabTime));
    }

    protected IEnumerator LetAnimationRunForTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        BackToIdle();
    }

    protected IEnumerator PlayCue(float time)
    {
        yield return new WaitForSeconds(time);
        DisableCues();
    }

    protected IEnumerator PlayWinLoseAfterGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        // The Static Vars win variable is set to true if the player has won
        // so the **enemy** plays the lose animation.
        if (StaticVars.win) {
            animator.Play(EnemyState.LOSE);
        } else {
            animator.Play(EnemyState.WIN);
        }
    }

    protected virtual void DisableCues() {
        shouldCueRightHitting = false;
        shouldCueJab = false;
    }

    protected virtual void EnableCues() {
        shouldCueRightHitting = true;
        shouldCueJab = true;
    }

    protected void BackToIdle() {
        animator.Play(EnemyState.IDLE);
        action = EnemyState.IDLE;
        EnableCues();
        shouldTakeTeamHealth = true;
        shouldPlayDodgeSound = true;
        enemyState = EnemyState.IDLE;
    }

    public virtual void TakeDamage(string side) {
        flashEffect.Flash();
    }

    public virtual void ReactToHit() {
        int randomValue = Random.Range(0, 99);

        if (enemyState == EnemyState.IDLE && randomValue < 30) {
            SetBlocking();
        }
    }

    protected abstract void Choose();

    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    protected virtual void Update()
    {
        // Do not update if game is over
        if (StaticVars.gameOver) {
            if (shouldPlayWinLoseAfterGameOver) {
                StartCoroutine(PlayWinLoseAfterGameOver(jabTime));
                shouldPlayWinLoseAfterGameOver = false;
            }
            return;
        }
        if (action == EnemyState.JAB) {
            OnJab();
        }
        if (action == EnemyState.RIGHT) {
            OnRight();
        }
        if (action == EnemyState.BLOCK) {
            OnBlock();
        }
        if (action == EnemyState.DODGE_LEFT) {
            OnDodgeLeft();
        }
        if (action == EnemyState.DODGE_RIGHT) {
            OnDodgeRight();
        }
    }

    public virtual string GetEnemyType() {
        return "Enemy";
    }
}
