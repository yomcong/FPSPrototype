using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSniperRifle : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject _muzzleFlashEffect;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform _casingSpawnPoint;
    [SerializeField]
    private Transform _bulletSpawnPoint;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip _audioClipFire;
    [SerializeField]
    private AudioClip _audioClipReloadPart1;
    [SerializeField]
    private AudioClip _audioClipReloadPart2;

    [Header("Aim UI")]
    [SerializeField]
    private Image _imageAim;
    [SerializeField]
    private Image _imageZoomAim;

    private void Awake()
    {
        Init();

        _weaponSetting.CurrentAmmo = _weaponSetting.MagCapacity;
    }

    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);

        _muzzleFlashEffect.SetActive(false);

        OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

        ResetVariables();

        _weaponSetting.IsAutomaticAttack = false;

        OnAutomaticEvent.Invoke(_weaponSetting.IsAutomaticAttack);
    }

    public override void StartReload()
    {
        if (_isReload || _isAttack || _animator.IsAimMode == true)
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
                _animator.Play(_animator.AnimParam.AimFire, -1, 0);
            }
            else
            {
                _animator.Play(_animator.AnimParam.Fire, -1, 0);
                StartCoroutine("OnMuzzleFlashEffect");
            }

            PlaySound(_audioClipFire);

            _casingMemoryPool.SpawnCasing(_casingSpawnPoint.position, transform.right);

            TwoStepRaycast();
        }
    }

    private IEnumerator AttackAnimation()
    {
        _isAttack = true;

        yield return new WaitForSeconds(1.2f);

        _isAttack = false;
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, _weaponSetting.AttackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * _weaponSetting.AttackDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * _weaponSetting.AttackDistance, Color.red);

        Vector3 attackDirection = (targetPoint - _bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(_bulletSpawnPoint.position, attackDirection, out hit, _weaponSetting.AttackDistance))
        {
            _impactMemoryPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<EnemyBase>().TakeDamage(_weaponSetting.Damage);
            }
            else if (hit.transform.CompareTag("InteractionObject"))
            {
                hit.transform.GetComponent<InteractionObjectBase>().TakeDamage(_weaponSetting.Damage);
            }

        }
        Debug.DrawRay(_bulletSpawnPoint.position, attackDirection * _weaponSetting.AttackDistance, Color.blue);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        _muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(0.1f);

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
                StopCoroutine("StartReloadSound");

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

        yield return new WaitForSeconds(1.7f);

        PlaySound(_audioClipReloadPart2);
    }

    private IEnumerator OnAimModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        _animator.IsAimMode = !_animator.IsAimMode;
        _imageAim.enabled = !_imageAim.enabled;
        _imageZoomAim.enabled = !_imageAim.enabled;

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
        _weaponSetting.MaxAmmo = _weaponSetting.MaxAmmo + 10 > _weaponSetting.MaxLimitAmmo
            ? _weaponSetting.MaxLimitAmmo : _weaponSetting.MaxAmmo + 10;

        OnAmmoEvent.Invoke(CurrentAmmo, MaxAmmo);
    }

    public override void IsAutomaticChange()
    {
        _weaponSetting.IsAutomaticAttack = false;
        OnAutomaticEvent.Invoke(_weaponSetting.IsAutomaticAttack);
    }
}
