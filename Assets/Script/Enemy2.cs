using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    // public Animator animator;
    // public GameLogic gameLogic;
    private float[] probs = new float[] {0.2f, 0.2f, 0.2f, 0.2f, 0.2f};

    protected override void Choose() {
        int randomValue = Random.Range(0, 99);
        if (randomValue < 45) {
            action = State.RIGHT;
        } else if (randomValue < 90) {
            action = State.JAB;
        } else if (randomValue < 95) {
            action = State.DODGE_LEFT;
        } else {
            action = State.DODGE_RIGHT;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        InvokeRepeating("Choose", 1.2f, 1.2f);
    }
}
