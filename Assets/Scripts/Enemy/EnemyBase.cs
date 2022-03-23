using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState { Idle = 0, Patrol, Cover, Crouch, chase }

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField]
    protected float targetDetectedRadius; //적 인식범위
    [SerializeField]
    protected float targetDetectedAngle; //적 인식각도
    [SerializeField]
    protected float detectedLimitRange;  //정찰 범위
    [SerializeField]
    protected float detectedInteractionObjectRadius;  //오브젝트 인식 범위
    [SerializeField]
    protected Transform _target;

    protected List<EnemyStateBase> _stateList = new List<EnemyStateBase>();
    protected EnemyFSM _enemyFSM;
    protected NavMeshAgent _navMeshAgent;
    protected Status _status;
    protected EnemyAnimationController _animator;

    protected Transform[] InteractionObejctPoint = new Transform[4];

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
        EnemyStateCover Cover = gameObject.AddComponent<EnemyStateCover>();
        EnemyStateCrouch Crouch = gameObject.AddComponent<EnemyStateCrouch>();
        EnemyStateChase chase = gameObject.AddComponent<EnemyStateChase>();

        _stateList.Add(idle);
        _stateList.Add(patrol);
        _stateList.Add(Cover);
        _stateList.Add(Crouch);
        _stateList.Add(chase);

        foreach (var iter in _stateList)
        {
            iter.Setup(gameObject, _target);
        }

        _enemyFSM.Setup(idle);
    }

    public void TakeDamage(int damage)
    {
        bool isDie = _status.DecreaseHP(damage);

        //hpBarSlider.value = (float)status.CurrentHP / status.MaxHP;

        if (isDie == true)
        {
            //enemyMemoryPool.DeactivateEnemy(gameObject);
            Debug.Log("enemy 죽음");
            _status.IncreaseHP(100);
        }
    }

}
