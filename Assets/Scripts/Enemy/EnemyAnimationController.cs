using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : AnimationController
{
    void Awake()
    {
        Setup();
    }

    public bool IsStanding
    {
        set => _animator.SetBool(_animationParam.IsStanding, value);
        get => _animator.GetBool(_animationParam.IsStanding);
    }
    public bool IsCover
    {
        set => _animator.SetBool(_animationParam.IsCover, value);
        get => _animator.GetBool(_animationParam.IsCover);
    }
    public bool IsCrouch
    {
        set => _animator.SetBool(_animationParam.IsCrouch, value);
        get => _animator.GetBool(_animationParam.IsCrouch);
    }

    public void IsFire()
    {
        _animator.SetTrigger(_animationParam.Fire);
    }

    public void IsIdle()
    {
        _animator.SetTrigger(_animationParam.Idle);
    }

}
