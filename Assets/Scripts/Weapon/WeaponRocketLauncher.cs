using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRocketLauncher : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject _muzzleFlashEffect;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip _audioClipFire;
    [SerializeField]
    private AudioClip _audioClipReloadPart1;
    [SerializeField]
    private AudioClip _audioClipReloadPart2;

    [SerializeField]
    protected GameObject _rocketPrefab;
    [SerializeField]
    protected Transform _rocketSpawnPoint;

    [Header("Aim UI")]
    [SerializeField]
    private Image _imageAim;

    private void Awake()
    {
        Init();

        _weaponSetting.CurrentAmmo = _weaponSetting.MagCapacity;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);

        ResetVariables();

        _muzzleFlashEffect.SetActive(false);

        OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

        OnAutomaticEvent.Invoke(_weaponSetting.IsAutomaticAttack);
    }

    public override void StartReload()
    {
        if (_isReload || _animator.IsAimMode == true)
        {
            return;
        }

        if (_weaponSetting.CurrentAmmo == _weaponSetting.MagCapacity || MaxAmmo <= 0)
        {
            return;
        }

        StopWeaponAction();

        StartCoroutine("OnReload");
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (_isReload || _isAimModeChange)
        {
            return;
        }

        if (type == 0)
        {
            OnAttack();
        }
        else
        {
            if (_isAttack)
            {
                return;
            }

            StartCoroutine("OnAimModeChange");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {

    }

    private void ResetVariables()
    {
        _isReload = false;
        _isAttack = false;
        _isAimModeChange = false;
        _weaponSetting.IsAutomaticAttack = false;

        _imageAim.enabled = false;
        _animator.IsAimMode = true;
        StartCoroutine("OnAimModeChange");
    }

    private void OnAttack()
    {
        if (Time.time - _lastAttackTime > _weaponSetting.AttackRate)
        {
            if (_animator.MoveSpeed > 0.5f)
            {
                return;
            }

            if (_weaponSetting.CurrentAmmo <= 0)
            {
                return;
            }

            StopCoroutine("AttackAnimation");
            StartCoroutine("AttackAnimation");

            _lastAttackTime = Time.time;

            _weaponSetting.CurrentAmmo--;
            OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

            if (_animator.IsAimMode)
            {
                StartCoroutine("OnAimModeChange");
            }
            else
            {
                StartCoroutine("OnMuzzleFlashEffect");
            }

            _animator.Play(_animator.AnimParam.Fire, -1, 0);

            PlaySound(_audioClipFire);

            LaunchedRocketProjectile();
        }
    }
    private IEnumerator AttackAnimation()
    {
        _isAttack = true;

        yield return new WaitForSeconds(0.6f);

        _isAttack = false;
    }

    private void LaunchedRocketProjectile()
    {
        GameObject grenadeClone = Instantiate(_rocketPrefab, _rocketSpawnPoint.position, transform.rotation);
        grenadeClone.GetComponent<ExplosionProjectile>().Setup(_weaponSetting.Damage, -transform.forward);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        _muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(_weaponSetting.AttackRate * 0.3f);

        _muzzleFlashEffect.SetActive(false);
    }
    private IEnumerator OnReload()
    {
        _isReload = true;

        _animator.OnReload();

        StartCoroutine("StartReloadSound");

        while (true)
        {
            if (_audioSource.isPlaying == false && _animator.CurrentAnimationIs(_animator.AnimParam.Movement))
            {
                _isReload = false;

                int requireAmmo = _weaponSetting.MagCapacity - CurrentAmmo;

                if (_weaponSetting.MaxAmmo < requireAmmo)
                {
                    requireAmmo = _weaponSetting.MaxAmmo;
                }
                _weaponSetting.MaxAmmo -= requireAmmo;
                _weaponSetting.CurrentAmmo += requireAmmo;

                OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator StartReloadSound()
    {
        PlaySound(_audioClipReloadPart1);

        yield return new WaitForSeconds(1.4f);

        PlaySound(_audioClipReloadPart2);

        yield return new WaitForSeconds(1.6f);

        PlaySound(_audioClipReloadPart2);

    }

    private IEnumerator OnAimModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        _animator.IsAimMode = !_animator.IsAimMode;
        _imageAim.enabled = !_imageAim.enabled;

        float start = _mainCamera.fieldOfView;
        float end = _animator.IsAimMode == true ? _aimModeFOV : _defaultModeFOV;

        _isAimModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            _mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        _isAimModeChange = false;
    }
    public override void IncreaseAmmo()
    {
        _weaponSetting.MaxAmmo = _weaponSetting.MaxAmmo + 2 > _weaponSetting.MaxLimitAmmo
            ? _weaponSetting.MaxLimitAmmo : _weaponSetting.MaxAmmo + 2;

        OnAmmoEvent.Invoke(CurrentAmmo, MaxAmmo);
    }

    public override void IsAutomaticChange()
    {
        _weaponSetting.IsAutomaticAttack = false;
        OnAutomaticEvent.Invoke(_weaponSetting.IsAutomaticAttack);
    }
}
