using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private void Awake()
    {
        Setup();
    }
    public bool IsAimMode
    {
        set => _animator.SetBool(_animationParam.IsAimMode, value);
        get => _animator.GetBool(_animationParam.IsAimMode);
    }
    public void OnJump()
    {
        _animator.SetTrigger(_animationParam.OnJump);
    }

    
}
