using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
public class AnimationParam
{
    public readonly int Fire = Animator.StringToHash("Fire");
    public readonly int Idle = Animator.StringToHash("Idle");
    public readonly int AimFire = Animator.StringToHash("AimFire");
    public readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    public readonly int OnReload = Animator.StringToHash("OnReload");
    public readonly int OnJump = Animator.StringToHash("OnJump");
    public readonly int IsAimMode = Animator.StringToHash("IsAimMode");
    public readonly int IsStanding = Animator.StringToHash("IsStanding");
    public readonly int IsCover = Animator.StringToHash("IsCover");
    public readonly int IsCrouch = Animator.StringToHash("IsCrouch");
    public readonly int Grenade = Animator.StringToHash("Grenade");
    public readonly int TrunAround = Animator.StringToHash("Trun");
    public readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
    public readonly int IsHit = Animator.StringToHash("Hit");
    public readonly int IsDie = Animator.StringToHash("Die");
    public readonly int IsCrouchDie = Animator.StringToHash("CrouchDie");

    public readonly string Movement = "Movement";
    public readonly string Trun = "Trun";
    public readonly string CrouchAutoFire = "CrouchAutoFire";
    public readonly string CoverFire = "CoverFire";
    public readonly string StandingFire = "StandingFire";
    public readonly string Hit = "Hit";
    public readonly string ThrowGrenade = "Grenade";
    public readonly string CrouchIdle = "CrouchIdle";
    public readonly string CrouchReload = "CrouchReload";
    public readonly string KnifeAttack = "MeleeAttack";

}