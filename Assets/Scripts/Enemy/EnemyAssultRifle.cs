using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAssultRifle : EnemyBase
{
    private enum ObstaclePointDir { Forward = 0, Right, Left, Back }

    private Transform _ObstacleObjectPoint;
    private CapsuleCollider _capsuleCollider;
    [SerializeField]
    private GameObject _textDamage;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Transform _textDamageSpawnPoint;

    private bool _inBattle = false;
    private bool _isDie = false;

    private void Awake()
    {
        Setup();

        _capsuleCollider = GetComponent<CapsuleCollider>();

        //_targetDetectedRadius = 15;
        //_targetDetectedAngle = 60;
        //_detectedLimitRange = 30;
        //_detectedObstacleObjectRadius = 3;
    }

    private void Start()
    {
        StartCoroutine("CalculateDistanceToTarget");
    }

    public override void TakeDamage(int damage)
    {
        bool isDie = _status.DecreaseHP(damage);

        GameObject textDamage = Instantiate(_textDamage, _textDamageSpawnPoint.position, Quaternion.identity, _canvas.transform);
        textDamage.GetComponent<DamageText>()?.Setup(damage, _canvas, _textDamageSpawnPoint.position);
        //hpBarSlider.value = (float)status.CurrentHP / status.MaxHP;

        if (isDie == true && _isDie == false)
        {
            //enemyMemoryPool.DeactivateEnemy(gameObject);
            _isDie = true;
            _capsuleCollider.enabled = false;

            _enemyFSM.IsDie();
        }
        else
        {
            _animator.Play(_animator.AnimParam.IsHit, -1, 0);
            StopCoroutine("HitLayerWeight");
            StartCoroutine("HitLayerWeight");
        }
    }

    private IEnumerator HitLayerWeight()
    {
        _animator.SetLayerWeight(1, 0.5f);

        yield return new WaitForSeconds(0.1f);

        _animator.SetLayerWeight(1, 0);
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

        LayerMask objectMask = (1 << LayerMask.NameToLayer("InteractionObject") | (1 << LayerMask.NameToLayer("ObstacleObject")));
        objectMask = ~objectMask;

        if (Vector3.Angle(transform.forward, dirToTarget) <= _targetDetectedAngle)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dirToTarget, out hit, _targetDetectedRadius, objectMask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    if (FindOfObstacleObject() == false)
                    {
                        EnemyFSM.SetState(_stateList[(int)EnemyState.Standing]);
                    }

                    _inBattle = true;
                }
            }
        }
    }

    private bool FindOfObstacleObject()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                        _detectedObstacleObjectRadius, transform.forward, 1, -1);

        foreach (var iter in hits)
        {
            if (iter.transform.CompareTag("ObstacleObject"))
            {
                if (iter.transform.GetComponent<ObstacleObject>()?.InCover == false)
                {
                    _interactObject = iter.transform.gameObject;
                    iter.transform.GetComponent<ObstacleObject>().InCover = true;
                }
                else
                {
                    continue;
                }

                float targetDistance = Vector3.Distance(_target.position, transform.position);

                Vector3 dirToTarget = (_target.position - iter.transform.position).normalized;

                Vector3 crossVec = Vector3.Cross(iter.transform.forward,
                                                    _target.position - iter.transform.position);

                float dot = Vector3.Dot(iter.transform.forward, dirToTarget);

                float cross = Vector3.Dot(Vector3.up, crossVec);

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

                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(_ObstacleObjectPoint.position);

                return true;
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectedObstacleObjectRadius);
    }
}
