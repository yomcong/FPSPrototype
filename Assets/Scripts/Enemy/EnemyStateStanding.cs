using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateStanding : EnemyStateBase
{
    private WaitForSeconds _attackDelaySeconds = new WaitForSeconds(1f);

    public override void StateEnter()
    {
        _navMeshAgent.speed = 0;
        _animator.MoveSpeed = 0;
        _navMeshAgent.ResetPath();

        StartCoroutine("StateAction");
        StartCoroutine("LookRotationToTarget");
    }

    public override IEnumerator StateAction()
    {
        _animator.IsStanding = true;

        yield return null;

        DoFire();

        while (true)
        {
            if (_isAttack)
            {
                yield return new WaitForSeconds(3f);

                _animator.IsIdle();
                _isAttack = false;
            }
            else
            {
                if (_currAttackCount >= _attackCount)
                {
                    // ∏Æ∆—≈‰∏µ
                    int temp = Random.Range(1, 5);

                    if (temp == 1)
                    {
                        _animator.ThrowGrenade();

                        yield return new WaitForSeconds(2.5f);

                        GrenadeInstantiate();
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
        _animator.IsStanding = false;
        _animator.IsIdle();

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
            if (_animator.CurrentAnimationIs(_animator.AnimParam.StandingFire))
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
            Vector3 to = new Vector3(_target.position.x, 0, _target.position.z);

            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

            transform.rotation = Quaternion.LookRotation(to - from);

            yield return null;
        }
    }
}
