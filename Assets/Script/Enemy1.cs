using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    // public Animator animator;
    // public GameLogic gameLogic;

    protected override void Choose() {
        action = actions[Random.Range(0, actions.Length)];
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        InvokeRepeating("Choose", 2.0f, 3.0f);
    }
}
