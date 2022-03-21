using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRocketLauncher : WeaponBase
{
    private void Awake()
    {
        Init();

        //casingMemoryPool = GetComponent<CasingMemoryPool>();
        //impactMemoryPool = GetComponent<ImpactMemoryPool>();

        //_mainCamera = Camera.main;

        _weaponSetting.CurrentAmmo = _weaponSetting.MagCapacity;
    }

    private void OnEnable()
    {
        //PlaySound(_audioClipTakeOutWeapon);

        //_muzzleFlashEffect.SetActive(false);

        OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

        //ResetVariables();
    }

    public override void StartReload()
    {
        throw new System.NotImplementedException();
    }

    public override void StartWeaponAction(int type = 0)
    {
        throw new System.NotImplementedException();
    }

    public override void StopWeaponAction(int type = 0)
    {
        throw new System.NotImplementedException();
    }
}
