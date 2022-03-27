using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    public override void StateEnter()
    {
        _isStanding = false;
        _isCover = false;
        _isCrouch = false;

        _navMeshAgent.speed = 0f;
        _animator.MoveSpeed = 0f;
        StartCoroutine("StateAction")
;
    }

    public override IEnumerator StateAction()
    {
        _animator.Play(_animator.AnimParam.TrunAround, -1, 0);

        //_animator.Play(_animator.AnimParam.Trun, -1, 0);


        StartCoroutine("TurnAround");

        yield return new WaitForSeconds(Random.Range(2, 5));

        StopCoroutine("TurnAround");

        _enemyFSM.SetState(_owner.GetComponent<EnemyBase>().StateList[(int)EnemyState.Patrol]);
    }

    public override void StateExit()
    {
        StopCoroutine("StateAction");
    }

    private IEnumerator TurnAround()
    {
        yield return null;

        while(_animator.CurrentAnimationIs(_animator.AnimParam.Trun))
        {
            transform.Rotate(new Vector3(0, 45 * Time.deltaTime, 0));

            yield return null;
        }

        yield break;
    }
}
