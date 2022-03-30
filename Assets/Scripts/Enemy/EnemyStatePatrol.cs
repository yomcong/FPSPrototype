using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol : EnemyStateBase
{
    public override void StateEnter()
    {
        _isStanding = false;
        _isCover = false;
        _isCrouch = false;

        _animator.MoveSpeed = _status.WalkSpeed;
        _navMeshAgent.speed = _status.WalkSpeed;

        StartCoroutine("StateAction");
    }

    public override IEnumerator StateAction()
    {
        float currentTime = 0;
        float maxTime = 10;

        _navMeshAgent.SetDestination(CalculatePatrolPosition());

        Vector3 to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            //목표위치도착 또는 10초이상 경과시 
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                _navMeshAgent.speed = 0;
                _animator.MoveSpeed = 0;
                _navMeshAgent.ResetPath();

                _enemyFSM.SetState(_owner.GetComponent<EnemyBase>().StateList[(int)EnemyState.Idle]);
                yield break;
            }

            yield return null;
        }
    }
    public override void StateExit()
    {
        StopAllCoroutines();

        //StopCoroutine("StateAction");
    }

    private Vector3 CalculatePatrolPosition()
    {
        float PatrolRadius = 10; //현재 위치를 원점으로 하는 원의 반지름 (정찰 범위);
        int PatrolJitter = 0; //선택된 각도
        int PatrolJitterMin = 0;
        int PatrolJitterMax = 360;

        // 맵 범위 밖으로 나가지 못하도록
        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;  //맵 크기 or 영역 크기

        // 랜덤하게 선택된 방향으로 이동.
        PatrolJitter = Random.Range(PatrolJitterMin, PatrolJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(PatrolRadius, PatrolJitter);

        // 생성된 목표위치가 자신의 이동구역을 벗어나지 않도록 조절
        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f,
                                                            rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;

        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f,
                                                            rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _navMeshAgent.destination - transform.position);
    }
}
