using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractRobotTutorialHelper : AbstractRobot
{

    protected Player _player;

    public AbstractRobotTutorialHelper(Player robot)
    {
        this._player = robot;
    }

    public void SetAbstractRobot(Player robot)
    {
        this._player = robot;
    }
    
    public override void OnJab(InputAction.CallbackContext context)
    {
        this._player.OnJab(context);
    }

    public override void OnRight(InputAction.CallbackContext context)
    {
        this._player.OnRight(context);
    }

    public override void OnDodgeRight(InputAction.CallbackContext context)
    {
        this._player.OnDodgeRight(context);
    }

    public override void OnDodgeLeft(InputAction.CallbackContext context)
    {
        this._player.OnDodgeLeft(context);
    }
}
