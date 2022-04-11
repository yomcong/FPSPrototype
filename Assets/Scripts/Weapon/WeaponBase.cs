using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Main = 0, Sub, Sub2, Special, Throw };

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class AutomaticEvent : UnityEngine.Events.UnityEvent<bool> { }

public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField]
    protected WeaponType _weaponType;
    [SerializeField]
    protected WeaponSetting _weaponSetting;
    [SerializeField]
    protected WeaponKnifeCollider _weaponKnifeCollider;

    [SerializeField]
    protected GameObject _grenadePrefab;
    [SerializeField]
    protected Transform _grenadeSpawnPoint;

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

    protected CasingMemoryPool _casingMemoryPool;
    protected ImpactMemoryPool _impactMemoryPool;
    protected Camera _mainCamera;
    protected AudioSource _audioSource;
    protected PlayerAnimationController _animator;
    protected Transform _knifeCollider;

    [HideInInspector]
    public AmmoEvent OnAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public AutomaticEvent OnAutomaticEvent = new AutomaticEvent();


    public PlayerAnimationController Animator => _animator;
    public WeaponNaming WeaponName => _weaponSetting.WeaponName;
    public int CurrentAmmo => _weaponSetting.CurrentAmmo;
    public int MaxAmmo => _weaponSetting.MaxAmmo;

    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();
    //public abstract void ThrowGrenade();

    public abstract void IsAutomaticChange();
    public abstract void IncreaseAmmo();

    protected void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<PlayerAnimationController>();
        _animator.Setup();
        _casingMemoryPool = GetComponent<CasingMemoryPool>();
        _impactMemoryPool = GetComponentInParent<ImpactMemoryPool>();
        _weaponKnifeCollider = transform.parent.GetComponentInChildren<WeaponKnifeCollider>();

        _mainCamera = Camera.main;
    }

    protected void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    public void ThrowGrenade()
    {
        if( _isAimModeChange || _isReload || _isAttack)
        {
            return;
        }

        StartCoroutine("GrenadeInstantiate");
        //StartCoroutine("GrenadeInstantiate", (grenadePrefab, grenadeSpawnPoint));
    }

    public IEnumerator GrenadeInstantiate()
    {
        _animator.Play(_animator.AnimParam.Grenade, -1, 0);

        yield return new WaitForSeconds(1f);

        GameObject grenadeClone = Instantiate(_grenadePrefab, _grenadeSpawnPoint.position, UnityEngine.Random.rotation);
        grenadeClone.GetComponent<ExplosionProjectile>().Setup(_weaponSetting.GrenadeDamage, -transform.forward);
    }

    public void MeleeAttack()
    {
        _animator.Play(_animator.AnimParam.MeleeAttack, -1, 0);

        StartCoroutine("MeleeAttackCollisionCheck");
    }

    private IEnumerator MeleeAttackCollisionCheck()
    {
        yield return new WaitForSeconds(0.1f);

        _weaponKnifeCollider.StartCollider(_weaponSetting.MeleeDamage);
    }

}
