using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCrouch : EnemyStateBase
{
    private int _currAttackCount = 0;

    private bool _isAttack = false;

    public override void StateEnter()
    {
        _currAttackCount = 0;
        StartCoroutine("TakeCover");

        //StartCoroutine("StateAction");
    }
    public override IEnumerator StateAction()
    {
        _animator.IsCrouch = true;

        yield return null;

        _animator.IsFire();
        _currAttackCount++;
        _isAttack = true;
        _attackStartTime = Time.time;
        float shotDelay = Time.time;
        while (true)
        {
            transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.eulerAngles = new Vector3(-10, transform.localEulerAngles.y, transform.localEulerAngles.z);

            if (_isAttack)
            {
                if (Time.time - _attackStartTime >= _attackTime)
                {
                    _animator.IsIdle();
                    _lastAttackTime = Time.time;
                    _isAttack = false;
                }
                else
                {
                    if(Time.time - shotDelay >= 1f )
                    {
                        shotDelay = Time.time;
                        ShotProjectile();
                    }
                }
            }
            else
            {
                if (_currAttackCount >= _attackCount)
                {
                    int temp = Random.Range(1, 5);

                    if (temp == 1)
                    {
                        _animator.ThrowGrenade();

                        yield return new WaitForSeconds(2.5f);
                    }

                    _animator.OnReload();

                    yield return new WaitForSeconds(5f);

                    _currAttackCount = 0;

                    DoFire();
                }
                else if (Time.time - _lastAttackTime >= _attackTime)
                {
                    DoFire();
                }
            }
            yield return null;
        }
    }

    private void DoFire()
    {
        _animator.IsFire();
        _attackStartTime = Time.time;
        _currAttackCount++;
        _isAttack = true;
    }
    public override void StateExit()
    {
        StopCoroutine("StateAction");
    }

    private IEnumerator TakeCover()
    {
        yield return null;

        while (true)
        {
            float currentTime = 0;
            float maxTime = 10;

            Vector3 to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            _animator.MoveSpeed = _status.RunSpeed;

            //목표위치도착 또는 10초이상 경과시 
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                _navMeshAgent.speed = 0;
                _animator.MoveSpeed = 0;
                _navMeshAgent.ResetPath();

                StartCoroutine("StateAction");

                yield break;
            }

            yield return null;
        }
    }

}
