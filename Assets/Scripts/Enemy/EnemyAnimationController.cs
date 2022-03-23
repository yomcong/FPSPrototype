using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public float MoveSpeed
    {
        set => animator.SetFloat("MovementSpeed", value);
        get => animator.GetFloat("MovementSpeed");
    }

    public float MovementX
    {
        set => animator.SetFloat("Movement X", Mathf.Clamp(value, -1, 1));
        get => animator.GetFloat("Movement X");
    }
    public float MovementZ
    {
        set => animator.SetFloat("Movement Z", Mathf.Clamp(value, -1, 1));
        get => animator.GetFloat("Movement Z");
    }

    public bool IsAimMode
    {
        set => animator.SetBool("isAimMode", value);
        get => animator.GetBool("isAimMode");
    }
    public bool IsCover
    {
        set => animator.SetBool("isAimMode", value);
        get => animator.GetBool("isAimMode");
    }
    public bool IsCrouch
    {
        set => animator.SetBool("isAimMode", value);
        get => animator.GetBool("isAimMode");
    }

    public void OnReload()
    {
        animator.SetTrigger("onReload");
    }

    public void IsFire()
    {
        animator.SetTrigger("Fire");
    }

    public void ThrowGrenade()
    {
        animator.SetTrigger("Grenade");
    }
    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void Play(string name, LayerMask layer, float normalizedTime)
    {
        animator.Play(name, layer, normalizedTime);
    }
}
