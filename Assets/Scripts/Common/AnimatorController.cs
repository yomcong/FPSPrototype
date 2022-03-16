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
        set => _animator.SetFloat("MovementSpeed", value);
        get => _animator.GetFloat("MovementSpeed");
    }

    public void OnReload()
    {
        _animator.SetTrigger("OnReload");
    }

    public bool IsAimMode
    {
        set => _animator.SetBool("IsAimMode", value);
        get => _animator.GetBool("IsAimMode");
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
