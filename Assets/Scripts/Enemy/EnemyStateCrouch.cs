using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCrouch : EnemyStateBase
{
    public override void StateEnter()
    {
        StartCoroutine("TakeCover");

        //StartCoroutine("StateAction");
    }
    public override IEnumerator StateAction()
    {
        _animator.IsCrouch = true;

        while (true)
        {
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.eulerAngles = new Vector3(-10, transform.localEulerAngles.y, transform.localEulerAngles.z);

            _animator.IsFire();

            yield return null;

        }

    }
    public override void StateExit()
    {
        StopCoroutine("StateAction");
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

            _animator.MoveSpeed = _status.RunSpeed;

            //목표위치도착 또는 10초이상 경과시 
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                _navMeshAgent.speed = 0;
                _animator.MoveSpeed = 0;
                _navMeshAgent.ResetPath();

                StartCoroutine("StateAction");

                yield break;
            }

            yield return null;
        }
    }
}
