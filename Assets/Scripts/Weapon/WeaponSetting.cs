using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponNaming { AssaultRifle = 0, SniperRifle, Pistol, Launcher }


[System.Serializable]
public struct WeaponSetting
{
    public WeaponNaming WeaponName;

    public int Damage;

    public int CurrentAmmo;
    public int MagCapacity;
    public int MaxAmmo;

    public float AttackRate;
    public float AttackDistance;

    public bool IsAutomaticAttack;

}