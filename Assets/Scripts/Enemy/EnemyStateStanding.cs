using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStanding : EnemyStateBase
{
    public override void StateAction()
    {
        StartCoroutine("StartBattal");
    }

    public override void StateEnter()
    {

    }

    public override void StateExit()
    {

    }

    private IEnumerator StartBattal()
    {
        yield return null;
    }
}
