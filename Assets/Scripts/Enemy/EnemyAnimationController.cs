using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public float MoveSpeed
    {
        set => _animator.SetFloat("MovementSpeed", value);
        get => _animator.GetFloat("MovementSpeed");
    }

    public float MovementX
    {
        set => _animator.SetFloat("Movement X", Mathf.Clamp(value, -1, 1));
        get => _animator.GetFloat("Movement X");
    }
    public float MovementZ
    {
        set => _animator.SetFloat("Movement Z", Mathf.Clamp(value, -1, 1));
        get => _animator.GetFloat("Movement Z");
    }

    public bool IsAimMode
    {
        set => _animator.SetBool("isAimMode", value);
        get => _animator.GetBool("isAimMode");
    }
    public bool IsCover
    {
        set => _animator.SetBool("isAimMode", value);
        get => _animator.GetBool("isAimMode");
    }
    public bool IsCrouch
    {
        set => _animator.SetBool("isAimMode", value);
        get => _animator.GetBool("isAimMode");
    }

    public void OnReload()
    {
        _animator.SetTrigger("onReload");
    }

    public void IsFire()
    {
        _animator.SetTrigger("Fire");
    }

    public void ThrowGrenade()
    {
        _animator.SetTrigger("Grenade");
    }
    public bool CurrentAnimationIs(string name)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void Play(string name, LayerMask layer, float normalizedTime)
    {
        _animator.Play(name, layer, normalizedTime);
    }
}
