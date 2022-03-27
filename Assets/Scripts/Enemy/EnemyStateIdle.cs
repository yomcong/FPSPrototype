using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    public override void StateEnter()
    {
        _isAimMode = false;
        _isCover = false;
        _isCrouch = false;

        _navMeshAgent.speed = 0f;
        _animator.MoveSpeed = 0f;
        StartCoroutine("StateAction")
;
    }

    public override IEnumerator StateAction()
    {
        _animator.Play("Trun", -1, 0);

        yield return new WaitForSeconds(Random.Range(2, 5));

        _enemyFSM.SetState(_owner.GetComponent<EnemyBase>().StateList[(int)EnemyState.Patrol]);
    }

    public override void StateExit()
    {
        StopCoroutine("StateAction");
    }

}
