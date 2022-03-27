using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCrouch : EnemyStateBase
{
    public override void StateEnter()
    {
        Debug.Log("crouch exit");
        StartCoroutine("StateAction");
    }
    public override IEnumerator StateAction()
    {
        Debug.Log("crouch exit");
        yield return null;
    }
    public override void StateExit()
    {
        Debug.Log("crouch exit");
        StopCoroutine("StateAction");
    }
}
