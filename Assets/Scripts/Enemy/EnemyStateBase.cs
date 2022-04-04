using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class InteractEvent : UnityEngine.Events.UnityEvent<bool> { }

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

    protected float _attackRange = 20f; //사정거리
    protected float _attackTime = 4f;   //공격 실행시간
    protected float _attackDelay = 2f;  //공격 대기시간

    protected int _grenadeDamage = 20;
    protected int _currAttackCount = 0;
    protected int _attackCount = 3;

    protected bool _isAttack = false;

    public InteractEvent OnInteractEvent = new InteractEvent();


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

    public void ShotProjectile()
    {
       GameObject clone = Instantiate(_owner.GetComponent<EnemyBase>().ProjectilePrefab,
           _owner.GetComponent<EnemyBase>().ProjectileSpawnPoint.position,
           transform.rotation);
        clone.GetComponent<EnemyProjectileMovement>().Setup(_target.position);
    }

    public void GrenadeInstantiate()
    {
        GameObject grenadeObject = Instantiate(_owner.GetComponent<EnemyBase>().GrendePrefab
            , _owner.GetComponent<EnemyBase>().GrenadeSpawnPoint.position, UnityEngine.Random.rotation);
        Vector3 GrenadeDir = (transform.forward + transform.up).normalized;
        grenadeObject.GetComponent<ExplosionProjectile>().Setup(_grenadeDamage, GrenadeDir);
    }

}
