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

    protected bool _isAimMode = false;
    protected bool _isCover = false;
    protected bool _isCrouch = false;

    [Header("Attack")]
    protected GameObject _projectilePrefab;
    protected Transform _projectileSpawnPoint;
    protected float _attackRange = 10;
    protected float _attackRate = 1;

    protected float _lastAttackTime = 0;

    public abstract void StateEnter();
    public abstract void StateAction();
    public abstract void StateExit();

    public void Setup(GameObject owner, Transform target)
    {
        this._owner = owner;

        _animator = owner.GetComponent<EnemyAnimationController>();
        _navMeshAgent = owner.GetComponent<NavMeshAgent>();
        _status = owner.GetComponent<Status>();
        _enemyFSM = owner.GetComponent<EnemyBase>().EnemyFSM;
        this._target = target.transform;
    }
}
