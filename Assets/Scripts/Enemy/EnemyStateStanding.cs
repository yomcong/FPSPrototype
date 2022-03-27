using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStanding : EnemyStateBase
{
    public override void StateEnter()
    {
        StartCoroutine("StateAction");
    }

    public override IEnumerator StateAction()
    {
        yield return null;
    }


    public override void StateExit()
    {
        StopCoroutine("StateAction");
    }
}
