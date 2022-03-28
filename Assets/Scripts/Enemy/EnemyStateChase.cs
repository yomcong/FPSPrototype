using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase : EnemyStateBase
{
    public override void StateEnter()
    {
        StartCoroutine("StateAction");
    }

    public override IEnumerator StateAction()
    {
        _navMeshAgent.speed = _status.RunSpeed;
        _animator.MoveSpeed = _status.RunSpeed;

        while (true)
        {
            _navMeshAgent.SetDestination(_target.position);

            Vector3 to = new Vector3(_target.position.x, 0, _target.position.z);

            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            transform.rotation = Quaternion.LookRotation(to - from);

            yield return null;
        }
    }

    public override void StateExit()
    {
        _navMeshAgent.speed = 0;
        _animator.MoveSpeed = 0;

        StopAllCoroutines();
        //StopCoroutine("StateAction");
    }

}
