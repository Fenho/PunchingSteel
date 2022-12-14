using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    // public Animator animator;
    // public GameLogic gameLogic;
    protected float HIT_TIME = 1.5f;

    protected override void Choose() {
        int randomValue = Random.Range(0, 99);
        if (randomValue < 45) {
            action = RobotState.RIGHT;
        } else if (randomValue < 90) {
            action = RobotState.JAB;
        } else if (randomValue < 95) {
            action = RobotState.DODGE_LEFT;
        } else {
            action = RobotState.DODGE_RIGHT;
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
}
