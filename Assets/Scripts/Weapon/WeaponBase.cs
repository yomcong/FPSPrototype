using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum WeaponType { Main = 0, Sub, Melee, Throw };

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    //[SerializeField]
    //protected WeaponType weaponType;
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

    protected AudioSource _audioSource;
    protected AnimatorController _animator;

    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    public AnimatorController Animator => _animator;
    public WeaponNaming WeaponName => _weaponSetting.weaponName;
    //public int CurrentMagazine => weaponSetting.currentMagazine;
    //public int MaxMagazine => weaponSetting.maxMagazine;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<AnimatorController>();
    }

    protected void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    
}