using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCover : EnemyStateBase
{
    public override void StateEnter()
    {
        Debug.Log("cover exit");
    }

    public override void StateAction()
    {
        Debug.Log("cover exit");
    }
    public override void StateExit()
    {
        Debug.Log("cover exit");
    }
}
