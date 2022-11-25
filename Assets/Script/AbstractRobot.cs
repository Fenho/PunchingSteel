using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbstractRobot : MonoBehaviour
{

    public abstract void OnJab(InputAction.CallbackContext context);

    public abstract void OnRight(InputAction.CallbackContext context);

    public abstract void OnDodgeRight(InputAction.CallbackContext context);

    public abstract void OnDodgeLeft(InputAction.CallbackContext context);
}
