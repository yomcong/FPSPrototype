using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponNaming { AssaultRifle = 0, Revolver, CombatKnife, HandGrenade }


[System.Serializable]
public struct WeaponSetting
{
    public WeaponNaming weaponName;

    public int damage;

    public int currentAmmo;
    public int maxAmmo;

    public float attackRate;
    public float attackDistance;

    public bool isAutomaticAttack;

}