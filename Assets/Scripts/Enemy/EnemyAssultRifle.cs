using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAssultRifle : EnemyBase
{
    private bool _inBattle = false;

    private Transform[] _interactionObejctPoint = new Transform[4];

    private void Awake()
    {
        Setup();
    }

    private void Start()
    {
        StartCoroutine("CalculateDistanceToTarget");
        //StartCoroutine(CalculateDistanceToTarget());

    }

    // Update is called once per frame
    private void Update()
    {
        _enemyFSM.StateUpdate();
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
        while (_target != null)
        {
            float targetDistance = Vector3.Distance(_target.position, transform.position);

            if (_inBattle == false)
            {
                if (targetDistance <= _targetDetectedRadius)
                {
                    if (FieldOfViewTargetCheck())
                    {
                        _inBattle = true;

                        if(FindOfInteractionObject())
                        {

                        }
                        else
                        {
                            _enemyFSM.SetState(_stateList[(int)EnemyState.Standing]);
                        }

                    }
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

    private bool FieldOfViewTargetCheck()
    {
        Vector3 dirToTarget = (_target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, dirToTarget) <= (_targetDetectedAngle / 2))
        {
            //Ray ray;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dirToTarget, out hit, _targetDetectedRadius, -1))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }

            }
        }

        return false;
    }

    private bool FindOfInteractionObject()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                        _detectedInteractionObjectRadius, transform.forward, 1, -1);
        
        foreach (var iter in hits)
        {
            if (iter.transform.CompareTag("InteractionObject"))
            {
                float targetDistance = Vector3.Distance(_target.position, transform.position);

                Vector3 dirToTarget = (_target.position - iter.transform.position).normalized;

                Vector3 crossVec = Vector3.Cross(iter.transform.forward,
                                                    _target.position - iter.transform.position);

                //Debug.Log($"이름 : {iter.transform.name}, " +
                //                    $"position : {iter.transform}, " +
                //                    $"내적 : {Vector3.Dot(iter.transform.forward, dirToTarget)}");
                //Debug.Log($"이름 : {iter.transform.name}, " +
                //                    $"position : {iter.transform}, " +
                //                    $"외적 : {Vector3.Dot(Vector3.up, crossVec)}");

                float dot = Vector3.Dot(iter.transform.forward, dirToTarget);

                float cross = Vector3.Dot(Vector3.up, crossVec);

                if (Mathf.Abs(dot) <= Mathf.Abs(cross))
                {
                    Debug.Log(" 내적이 더 적음. ");
                    if (cross < 0)
                    {
                        _interactionObejctPoint[1] = iter.transform.GetChild(1);

                        Debug.Log($"이름 : {_interactionObejctPoint[1].name}, " +
                                   $"position : {_interactionObejctPoint[1].position}, ");

                    }
                    else
                    {
                        _interactionObejctPoint[2] = iter.transform.GetChild(2);

                        Debug.Log($"이름 : {_interactionObejctPoint[2].name}, " +
                                   $"position : {_interactionObejctPoint[2].position}, ");
                    }
                }
                else
                {
                    Debug.Log(" 외적이 더 적음. ");
                    if (dot < 0)
                    {
                        _interactionObejctPoint[0] = iter.transform.GetChild(0);

                        Debug.Log($"이름 : {_interactionObejctPoint[0].name}, " +
                                   $"position : {_interactionObejctPoint[0].position}, ");
                    }
                    else
                    {
                        _interactionObejctPoint[3] = iter.transform.GetChild(3);

                        Debug.Log($"이름 : {_interactionObejctPoint[3].name}, " +
                                   $"position : {_interactionObejctPoint[3].position}, ");
                    }
                }
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _targetDetectedRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectedLimitRange);
    }
}
