using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationParam
{
    [HideInInspector] public readonly int Fire = Animator.StringToHash("Fire");
    [HideInInspector] public readonly int Idle = Animator.StringToHash("Idle");
    [HideInInspector] public readonly int AimFire = Animator.StringToHash("AimFire");
    [HideInInspector] public readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    [HideInInspector] public readonly int OnReload = Animator.StringToHash("OnReload");
    [HideInInspector] public readonly int OnJump = Animator.StringToHash("OnJump");
    [HideInInspector] public readonly int IsAimMode = Animator.StringToHash("IsAimMode");
    [HideInInspector] public readonly int IsStanding = Animator.StringToHash("IsStanding");
    [HideInInspector] public readonly int IsCover = Animator.StringToHash("IsCover");
    [HideInInspector] public readonly int IsCrouch = Animator.StringToHash("IsCrouch");
    [HideInInspector] public readonly int Grenade = Animator.StringToHash("Grenade");
    [HideInInspector] public readonly int TrunAround = Animator.StringToHash("Trun");
    [HideInInspector] public readonly int Hit = Animator.StringToHash("Hit");

    public readonly string Movement = "Movement";
    public readonly string Trun = "Trun";
    public readonly string CrouchAutoFire = "CrouchAutoFire";
    public readonly string CoverFire = "CoverFire";
    public readonly string StandingFire = "StandingFire";
    
}