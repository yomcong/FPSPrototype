using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCover : EnemyStateBase
{
    private WaitForSeconds _attackDelaySeconds = new WaitForSeconds(1f);

    public override void StateEnter()
    {
        StartCoroutine("TakeCover");
    }

    public override IEnumerator StateAction()
    {
        _animator.IsCover = true;

        yield return null;

        DoFire();

        while (true)
        {
            if (_isAttack)
            {
                yield return new WaitForSeconds(4f);

                _animator.IsIdle();
                _lastAttackTime = Time.time;
                _isAttack = false;
            }
            else
            {
                if (_currAttackCount >= _attackCount)
                {
                    // 리팩토링
                    int temp = Random.Range(1, 5);

                    if (temp == 1)
                    {
                        _animator.ThrowGrenade();

                        yield return new WaitForSeconds(2.5f);
                    }
                    _animator.OnReload();

                    yield return new WaitForSeconds(5f);

                    _currAttackCount = 0;
                }
                yield return new WaitForSeconds(2f);

                DoFire();
            }
            yield return null;
        }
    }
    public override void StateExit()
    {

        StopCoroutine("LookRotationToTarget");
        StopAllCoroutines();

        //StopCoroutine("StateAction");
    }

    private void DoFire()
    {
        _animator.IsFire();
        _attackStartTime = Time.time;
        _currAttackCount++;
        _isAttack = true;

        StopCoroutine("LaunchedProjectile");
        StartCoroutine("LaunchedProjectile");
    }
    private IEnumerator LaunchedProjectile()
    {
        yield return _attackDelaySeconds;

        while (true)
        {
            if (_animator.CurrentAnimationIs(_animator.AnimParam.CrouchAutoFire))
            {
                ShotProjectile();

                yield return _attackDelaySeconds;
            }

            yield return null;
        }
    }
    private IEnumerator LookRotationToTarget()
    {
        while (true)
        {
            Vector3 to = new Vector3(-_target.position.x, 0, -_target.position.z);

            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            transform.rotation = Quaternion.LookRotation(to - from);

            yield return null;
        }
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

            _navMeshAgent.speed = _status.RunSpeed;
            _animator.MoveSpeed = _status.RunSpeed;

            //목표위치도착 또는 10초이상 경과시 
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                _navMeshAgent.speed = 0;
                _animator.MoveSpeed = 0;
                _navMeshAgent.ResetPath();

                StartCoroutine("StateAction");
                StartCoroutine("LookRotationToTarget");
                yield break;
            }

            yield return null;
        }
    }
}
