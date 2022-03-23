using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAssultRifle : EnemyBase
{
    private bool inBattle = false;

    private void Awake()
    {
        Setup();
    }

    private void Start()
    {
        //StartCoroutine("CalculateDistanceToTarget");
        StartCoroutine(CalculateDistanceToTarget());

    }

    // Update is called once per frame
    private void Update()
    {
        _enemyFSM.StateUpdate();
    }


    private IEnumerator CalculateDistanceToTarget()
    {
        while (_target != null)
        {
            float targetDistance = Vector3.Distance(_target.position, transform.position);

            if (inBattle == false)
            {
                if (targetDistance <= targetDetectedRadius)
                {
                    if (FieldOfViewTargetCheck())
                    {
                        inBattle = true;

                        FindOfInteractionObject();

                    }
                    //else
                    //{
                    //    enemyFSM.SetState(stateList[(int)EnemyState.chase]);
                    //}

                }
                else
                {
                    if (_animator.MoveSpeed <= 0)
                    {
                        _enemyFSM.SetState(_stateList[(int)EnemyState.Idle]);
                    }
                }
            }
            else
            {
                if (targetDistance >= detectedLimitRange)
                {
                    inBattle = false;
                    _enemyFSM.SetState(_stateList[(int)EnemyState.Idle]);
                }
            }


            yield return null;
        }
    }

    private bool FieldOfViewTargetCheck()
    {
        Vector3 dirToTarget = (_target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, dirToTarget) <= (targetDetectedAngle / 2))
        {
            Ray ray;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dirToTarget, out hit, targetDetectedRadius, -1))
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
                        detectedInteractionObjectRadius, transform.forward, 1, -1);

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
                        InteractionObejctPoint[1] = iter.transform.GetChild(1);

                        Debug.Log($"이름 : {InteractionObejctPoint[1].name}, " +
                                   $"position : {InteractionObejctPoint[1].position}, ");

                    }
                    else
                    {
                        InteractionObejctPoint[2] = iter.transform.GetChild(2);

                        Debug.Log($"이름 : {InteractionObejctPoint[2].name}, " +
                                   $"position : {InteractionObejctPoint[2].position}, ");
                    }
                }
                else
                {
                    Debug.Log(" 외적이 더 적음. ");
                    if (dot < 0)
                    {
                        InteractionObejctPoint[0] = iter.transform.GetChild(0);

                        Debug.Log($"이름 : {InteractionObejctPoint[0].name}, " +
                                   $"position : {InteractionObejctPoint[0].position}, ");
                    }
                    else
                    {
                        InteractionObejctPoint[3] = iter.transform.GetChild(3);

                        Debug.Log($"이름 : {InteractionObejctPoint[3].name}, " +
                                   $"position : {InteractionObejctPoint[3].position}, ");
                    }
                }
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetDetectedRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectedLimitRange);

    }
}
