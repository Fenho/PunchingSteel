using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBoxEnemy : Enemy
{
    private bool shouldCueHeadButt = true;
    private bool shouldCueDouble = true;
    private bool shouldCueTriple = true;

    private int TRIPLE_DAMAGE = 10;
    private int DOUBLE_DAMAGE = 10;
    private int HEAD_BUTT_DAMAGE = 10;

    protected float HIT_TIME = 1.5f;

    public override string GetEnemyType() {
        return "HeadBoxEnemy";
    }

    private void OnHeadButt() {
        enemyState = EnemyState.HEAD_BUTT;
        if (shouldCueHeadButt) {
            animator.Play(EnemyState.HEAD_BUTT_CUE);
            StartCoroutine(PlayCue(cueTime));
        } else {
            animator.Play(EnemyState.HEAD_BUTT);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(HEAD_BUTT_DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    StaticVars.addPointsByType("LeftJabEnemy");
                    audioSource.PlayOneShot(punchSound1, volume); // Sonido debería ser distinto, capaz uno metálico
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

    private void OnDouble() {
        enemyState = EnemyState.DOUBLE;
        if (shouldCueDouble) {
            animator.Play(EnemyState.DOUBLE_CUE);
            StartCoroutine(PlayCue(cueTime));
        } else {
            animator.Play(EnemyState.DOUBLE);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(DOUBLE_DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    StaticVars.addPointsByType("LeftJabEnemy");
                    audioSource.PlayOneShot(punchSound1, volume); // Sonido debería ser distinto, capaz uno metálico
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

    private void OnTriple() {
        enemyState = EnemyState.TRIPLE;
        if (shouldCueTriple) {
            animator.Play(EnemyState.TRIPLE_CUE);
            StartCoroutine(PlayCue(cueTime));
        } else {
            animator.Play(EnemyState.TRIPLE);
            if (shouldTakeTeamHealth) {
                GameLogic.PunchResult punchResult = gameLogic.TakeDamageTeam(TRIPLE_DAMAGE);
                if (punchResult == GameLogic.PunchResult.HIT) {
                    StaticVars.addPointsByType("LeftJabEnemy");
                    audioSource.PlayOneShot(punchSound1, volume); // Sonido debería ser distinto, capaz uno metálico
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

    protected override void DisableCues() {
        shouldCueRightHitting = false;
        shouldCueJab = false;
        shouldCueHeadButt = false;
        shouldCueDouble = false;
        shouldCueTriple = false;
    }

    protected override void EnableCues() {
        shouldCueRightHitting = true;
        shouldCueJab = true;
        shouldCueHeadButt = true;
        shouldCueDouble = true;
        shouldCueTriple = true;
    }


    protected override void Choose() {
        int randomValue = Random.Range(0, 99);
        if (randomValue < 50) {
            action = EnemyState.HEAD_BUTT;
        } else if (randomValue < 70) {
            action = EnemyState.DOUBLE;
        } else if (randomValue < 80) {
            action = EnemyState.TRIPLE;
        } else if (randomValue < 90) {
            action = EnemyState.JAB;
        }else {
            action = EnemyState.RIGHT;
        }
    }

    protected IEnumerator RandomHitTime()
    {
        float time = Random.Range(jabTime + cueTime, HIT_TIME);
        yield return new WaitForSeconds(time);
        Choose();
    }

    protected void CallRandomHit() {
        StartCoroutine(RandomHitTime());
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        InvokeRepeating("CallRandomHit", 1f, HIT_TIME);
    }

    // Update is called once per frame
    protected override void Update()
    {
         // Do not update if game is over
        if (StaticVars.gameOver) {
            if (shouldPlayWinLoseAfterGameOver) {
                // Inherited from Enemy
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
        if (action == EnemyState.HEAD_BUTT) {
            OnHeadButt();
        }
        if (action == EnemyState.DOUBLE) {
            OnDouble();
        }
        if (action == EnemyState.TRIPLE) {
            OnTriple();
        }
        // if (action == EnemyState.DODGE_LEFT) {
        //     OnDodgeLeft();
        // }
        // if (action == EnemyState.DODGE_RIGHT) {
        //     OnDodgeRight();
        // }
    }
}
