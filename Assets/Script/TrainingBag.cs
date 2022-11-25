using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBag : Enemy
{
    public override void TakeDamage(string side)
    {
        flashEffect.Flash();
        if (side == "Right") {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
        animator.Play($"damage{side}");
        StartCoroutine(LetAnimationRunForTime(cueSpeed + 0.5f));
    }

    public override void ReactToHit() {
        return;
    }


    // Start is called before the first frame update
    override protected void Start()
    {
        
    }

    override protected void Choose()
    {

    }

    override public string GetType()
    {
        return "TrainingBag";
    }
}
