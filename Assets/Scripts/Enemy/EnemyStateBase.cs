using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateBase : MonoBehaviour
{
    protected GameObject _owner = null;

    protected EnemyAnimationController _animator;
    protected NavMeshAgent _navMeshAgent;
    protected Status _status;
    protected Transform _target;
    protected EnemyFSM _enemyFSM;

    protected bool _isStanding = false;
    protected bool _isCover = false;
    protected bool _isCrouch = false;

    protected float _attackRange = 20f;  //사정거리
    protected float _attackTime = 4f;   //공격 실행시간
    protected float _attackDelay = 2f;   //공격 대기시간
    protected float _lastAttackTime = 0f;
    protected float _attackStartTime = 0f;

    protected int _attackCount = 5;


    public abstract void StateEnter();
    public abstract IEnumerator StateAction();
    public abstract void StateExit();

    public void Setup(GameObject owner, Transform target)
    {
        _owner = owner;

        _animator = owner.GetComponent<EnemyAnimationController>();
        _navMeshAgent = owner.GetComponent<NavMeshAgent>();
        _status = owner.GetComponent<Status>();
        _enemyFSM = owner.GetComponent<EnemyBase>().EnemyFSM;
        _target = target.transform;
    }
    private IEnumerator LookRotationToTarget()
    {
        while (true)
        {
            Vector3 to = new Vector3(_target.position.x, 0, _target.position.z);

            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            transform.rotation = Quaternion.LookRotation(to - from);

            yield return null;
        }
    }
    protected void StartLookRotationToTarget()
    {
        StartCoroutine("LookRotationToTarget");
    }

    protected void StopLookRotationToTarget()
    {
        StopCoroutine("LookRotationToTarget");
    }

    public void ShotProjectile()
    {
       GameObject clone = Instantiate(_owner.GetComponent<EnemyBase>().ProjectilePrefab,
           _owner.GetComponent<EnemyBase>().ProjectileSpawnPoint.position,
           transform.rotation);
        clone.GetComponent<ProjectileMovement>().Setup(_target.position);
    }
}
