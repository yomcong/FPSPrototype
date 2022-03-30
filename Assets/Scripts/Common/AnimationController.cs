using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    protected Animator _animator;
    [SerializeField]
    protected AnimationParam _animationParam = new AnimationParam();

    public AnimationParam AnimParam => _animationParam;

    public void Setup()
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
    public void ThrowGrenade()
    {
        _animator.SetTrigger(_animationParam.Grenade);
    }

    public void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
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
