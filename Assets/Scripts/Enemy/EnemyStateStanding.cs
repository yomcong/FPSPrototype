using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStanding : EnemyStateBase
{
    public override void StateEnter()
    {
        Debug.Log("Standing Enter");

        StartCoroutine("StateAction");
    }

    public override IEnumerator StateAction()
    {
        Debug.Log("Standing Action");

        yield return null;
    }


    public override void StateExit()
    {
        Debug.Log("Standing Exit");

        StopAllCoroutines();

        //StopCoroutine("StateAction");
    }

    private IEnumerator TakeCover()
    {
        yield return null;

        while (true)
        {
            float currentTime = 0;
            float maxTime = 10;

            Vector3 to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            _navMeshAgent.speed = _status.RunSpeed;
            _animator.MoveSpeed = _status.RunSpeed;

            //목표위치도착 또는 10초이상 경과시 
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                _navMeshAgent.speed = 0;
                _animator.MoveSpeed = 0;
                _navMeshAgent.ResetPath();

                StartCoroutine("StateAction");
                StartCoroutine("LookRotationToTarget");
                yield break;
            }

            yield return null;
        }
    }
}
