using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState { Idle = 0, Patrol, Cover, Crouch, Standing, Chase }

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Target")]
    [SerializeField]
    protected float _targetDetectedRadius; //적 인식범위
    [SerializeField]
    protected float _targetDetectedAngle; //적 인식각도
    [SerializeField]
    protected float _detectedLimitRange;  //정찰 범위
    [SerializeField]
    protected float _detectedObstacleObjectRadius;  //오브젝트 인식 범위
    [SerializeField]
    protected Transform _target;

    protected List<EnemyStateBase> _stateList = new List<EnemyStateBase>();
    protected EnemyFSM _enemyFSM;
    protected NavMeshAgent _navMeshAgent;
    protected Status _status;
    protected EnemyAnimationController _animator;

    [Header("Attack")]
    [SerializeField]
    protected GameObject _projectilePrefab;
    [SerializeField]
    protected Transform _projectileSpawnPoint;

    public GameObject ProjectilePrefab => _projectilePrefab;
    public Transform ProjectileSpawnPoint => _projectileSpawnPoint;

    public Transform Target
    {
        get => _target;
    }

    public List<EnemyStateBase> StateList
    {
        get => _stateList;
    }

    public EnemyFSM EnemyFSM
    {
        get => _enemyFSM;
    }
    protected void Setup()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimationController>();
        _enemyFSM = gameObject.AddComponent<EnemyFSM>();
        _status = GetComponent<Status>();

        EnemyStateIdle idle = gameObject.AddComponent<EnemyStateIdle>();
        EnemyStatePatrol patrol = gameObject.AddComponent<EnemyStatePatrol>();
        EnemyStateCover cover = gameObject.AddComponent<EnemyStateCover>();
        EnemyStateCrouch crouch = gameObject.AddComponent<EnemyStateCrouch>();
        EnemyStateStanding standing = gameObject.AddComponent<EnemyStateStanding>();
        EnemyStateChase chase = gameObject.AddComponent<EnemyStateChase>();

        _stateList.Add(idle);
        _stateList.Add(patrol);
        _stateList.Add(cover);
        _stateList.Add(crouch);
        _stateList.Add(standing);
        _stateList.Add(chase);

        foreach (var iter in _stateList)
        {
            iter.Setup(gameObject, _target);
        }

        _enemyFSM.Setup(idle);
    }

    public abstract void TakeDamage(int damage);

    //public void TakeDamage(int damage)
    //{
    //    bool isDie = _status.DecreaseHP(damage);

    //    //hpBarSlider.value = (float)status.CurrentHP / status.MaxHP;

    //    if (isDie == true)
    //    {
    //        //enemyMemoryPool.DeactivateEnemy(gameObject);
    //        _status.IncreaseHP(100);
    //    }
    //}

}
