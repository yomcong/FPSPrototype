using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Main = 0, Sub, Sub2, Special, Throw };

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField]
    protected WeaponType weaponType;
    [SerializeField]
    protected WeaponSetting _weaponSetting;

    protected float _lastAttackTime = 0;

    [SerializeField]
    protected bool _isReload = false;
    [SerializeField]
    protected bool _isAttack = false;
    [SerializeField]
    protected bool _isAimModeChange = false;

    [SerializeField]
    protected float _defaultModeFOV = 60;
    [SerializeField]
    protected float _aimModeFOV = 30;

    protected CasingMemoryPool casingMemoryPool;
    protected ImpactMemoryPool impactMemoryPool;
    protected Camera _mainCamera;
    protected AudioSource _audioSource;
    protected AnimatorController _animator;

    [HideInInspector]
    public AmmoEvent OnAmmoEvent = new AmmoEvent();

    public AnimatorController Animator => _animator;
    public WeaponNaming WeaponName => _weaponSetting.WeaponName;
    public int CurrentAmmo => _weaponSetting.CurrentAmmo;
    public int MaxAmmo => _weaponSetting.MagCapacity;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<AnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponentInParent<ImpactMemoryPool>();

        _mainCamera = Camera.main;
    }

    protected void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void IncreaseAmmo(int ammo)
    {
        _weaponSetting.CurrentAmmo =  CurrentAmmo + ammo > MaxAmmo ? MaxAmmo : CurrentAmmo + ammo;

        OnAmmoEvent.Invoke(CurrentAmmo, MaxAmmo);
    }

}
