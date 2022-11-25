using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotTutorialHelper : AbstractRobotTutorialHelper
{
    [SerializeField] protected GameObject robotGo;
    protected Player robot;

    public void Awake() {
        robotGo = GameObject.Find("Robot");
        if (robotGo != null) {
            robot = robotGo.GetComponent<Player>();
        }
    }

    public RobotTutorialHelper(Player robot) : base(robot)
    {
        // this._robot = robot;
    }

    // protected override void Update()
    // {
    //     Regen();
    //     if (trainer.trainerState == State.BLOCK) {
    //         playerState = teamState = State.BLOCK;
    //         animator.Play(State.BLOCK);
    //     }
    //     if (trainer.trainerState != State.BLOCK && !(DoingSomething() && !playerState.Equals(State.BLOCK))) {
    //         playerState = teamState = State.IDLE;
    //         animator.Play(State.IDLE);
    //     }
    // }
    public override void OnJab(InputAction.CallbackContext context)
    {
        Debug.Log("RobotTutorialHelper.OnJab");
        base.OnJab(context);
        
    }
}
