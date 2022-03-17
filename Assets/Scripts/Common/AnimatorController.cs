using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;

    [Serializable]
    public class AnimationParam
    {
        public int FireHash = Animator.StringToHash("Fire");
        public int AimFireHash = Animator.StringToHash("AimFire");
        public int MovementSpeed = Animator.StringToHash("MovementSpeed");
        public int OnReload = Animator.StringToHash("OnReload");
        public int OnJump = Animator.StringToHash("OnJump");
        public int IsAimMode = Animator.StringToHash("IsAimMode");

        public string Movement = "Movement";
        
    }

    [SerializeField] private AnimationParam _animationParam = new AnimationParam();

    public AnimationParam AnimParam => _animationParam;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public float MoveSpeed
    {
        set => _animator.SetFloat(_animationParam.MovementSpeed, value);
        get => _animator.GetFloat(_animationParam.MovementSpeed);
    }

    public void OnReload()
    {
        _animator.SetTrigger(_animationParam.OnReload);
    }
    public void OnJump()
    {
        _animator.SetTrigger(_animationParam.OnJump);
    }

    public bool IsAimMode
    {
        set => _animator.SetBool(_animationParam.IsAimMode, value);
        get => _animator.GetBool(_animationParam.IsAimMode);
    }


    public void Play(string stateName, int layer, float normalizedTime)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }
    public void Play(int stateName, int layer, float normalizedTime)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }

    public bool CurrentAnimationIs(string name)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void SetFloat(string paramName, float value)
    {
        _animator.SetFloat(paramName, value);
    }
}
