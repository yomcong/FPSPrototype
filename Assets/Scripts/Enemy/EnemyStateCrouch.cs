using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCrouch : EnemyStateBase
{
    private WaitForSeconds _attackDelaySeconds = new WaitForSeconds(1f);

    public override void StateEnter()
    {
        _currAttackCount = 0;

        StartCoroutine("TakeCover");
    }

    public override IEnumerator StateAction()
    {
        _animator.IsCrouch = true;

        yield return null;

        DoFire();

        while (true)
        {
            if (_isAttack)
            {
                yield return new WaitForSeconds(4f);

                _animator.IsIdle();
                _isAttack = false;
            }
            else
            {
                if (_currAttackCount >= _attackCount)
                {
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
        _animator.IsIdle();
        _animator.IsCover = false;
        OnInteractEvent.Invoke(false);

        StopCoroutine("LookRotationToTarget");
        StopAllCoroutines();
    }

    private void DoFire()
    {
        _animator.IsFire();
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

}
