using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum ObstaclePointDir { Forward = 0, Right, Left, Back }

public class EnemyAssultRifle : EnemyBase
{
    private bool _inBattle = false;

    private Transform _ObstacleObjectPoint;

    private void Awake()
    {
        Setup();

        _targetDetectedRadius = 15;
        _targetDetectedAngle = 60;
        _detectedLimitRange = 30;
        _detectedObstacleObjectRadius = 10;
    }

    private void Start()
    {
        StartCoroutine("CalculateDistanceToTarget");
        //StartCoroutine(CalculateDistanceToTarget());

    }

    private void Update()
    {

    }

    public override void TakeDamage(int damage)
    {
        bool isDie = _status.DecreaseHP(damage);

        //hpBarSlider.value = (float)status.CurrentHP / status.MaxHP;

        if (isDie == true)
        {
            //enemyMemoryPool.DeactivateEnemy(gameObject);
            _status.IncreaseHP(100);
        }
    }

    private IEnumerator CalculateDistanceToTarget()
    {
        yield return null;

        while (_target != null)
        {
            float targetDistance = Vector3.Distance(_target.position, transform.position);

            if (_inBattle == false)
            {
                if (targetDistance <= _targetDetectedRadius)
                {
                    FieldOfViewTargetCheck();
                }
            }
            else
            {
                if (targetDistance >= _detectedLimitRange)
                {
                    _inBattle = false;
                    _enemyFSM.SetState(_stateList[(int)EnemyState.Idle]);
                }
            }

            yield return null;
        }
    }

    private void FieldOfViewTargetCheck()
    {
        Vector3 dirToTarget = (_target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, dirToTarget) <= _targetDetectedAngle)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dirToTarget, out hit, _targetDetectedRadius, -1))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    FindOfObstacleObject();

                    _inBattle = true;
                }
            }
        }
    }

    private void FindOfObstacleObject()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                        _detectedObstacleObjectRadius, transform.forward, 1, -1);

        foreach (var iter in hits)
        {
            if (iter.transform.CompareTag("ObstacleObject"))
            {
                float targetDistance = Vector3.Distance(_target.position, transform.position);

                Vector3 dirToTarget = (_target.position - iter.transform.position).normalized;

                Vector3 crossVec = Vector3.Cross(iter.transform.forward,
                                                    _target.position - iter.transform.position);

                float dot = Vector3.Dot(iter.transform.forward, dirToTarget);

                float cross = Vector3.Dot(Vector3.up, crossVec);

                //if (dot <= 0)
                //{
                //    continue;
                //}

                if (dot <= Mathf.Abs(cross))
                {
                    if (cross < 0)
                    {
                        _ObstacleObjectPoint = iter.transform.GetChild((int)ObstaclePointDir.Right);
                    }
                    else
                    {
                        _ObstacleObjectPoint = iter.transform.GetChild((int)ObstaclePointDir.Left);
                    }
                }
                else
                {
                    if (dot < 0)
                    {
                        _ObstacleObjectPoint = iter.transform.GetChild((int)ObstaclePointDir.Forward);
                    }
                    else
                    {
                        _ObstacleObjectPoint = iter.transform.GetChild((int)ObstaclePointDir.Back);
                    }
                }

                if (iter.transform.localScale.y <= 1)
                {
                    EnemyFSM.SetState(_stateList[(int)EnemyState.Crouch]);
                }
                else if (iter.transform.localScale.y > 1)
                {
                    EnemyFSM.SetState(_stateList[(int)EnemyState.Cover]);
                }
                else
                {
                    EnemyFSM.SetState(_stateList[(int)EnemyState.Standing]);
                }

                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(_ObstacleObjectPoint.position);

                return;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _targetDetectedRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectedLimitRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectedObstacleObjectRadius);
    }
}
