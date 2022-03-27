using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCover : EnemyStateBase
{
    public override void StateEnter()
    {
        Debug.Log("cover exit");
        StartCoroutine("StateAction");
    }

    public override IEnumerator StateAction()
    {
        Debug.Log("cover exit");
        yield return null; 
    }
    public override void StateExit()
    {
        Debug.Log("cover exit");
        StopCoroutine("StateAction");

    }
}
