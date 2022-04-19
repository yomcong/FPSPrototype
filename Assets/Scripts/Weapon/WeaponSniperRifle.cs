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
    [SerializeField]
    protected GameObject _projectilePrefab;
    [SerializeField]
    protected Transform _projectileSpawnPoint;


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

        ResetVariables();

        _muzzleFlashEffect.SetActive(false);

        OnAmmoEvent.Invoke(_weaponSetting.CurrentAmmo, _weaponSetting.MaxAmmo);

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

            _casingMemoryPool.SpawnCasing(_casingSpawnPoint.position, transform.right);

            launchProjectile();
        }
    }

    private IEnumerator AttackAnimation()
    {
        _isAttack = true;

        yield return new WaitForSeconds(1.2f);

        _isAttack = false;
    }

    private void launchProjectile()
    {
        Ray ray;
        RaycastHit hit;
        LayerMask playerMask = (1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Trigger")));
        playerMask = ~playerMask;
        Vector3 targetPoint = Vector3.zero;

        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, _weaponSetting.AttackDistance, playerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * _weaponSetting.AttackDistance;
        }

        GameObject clone = Instantiate(_projectilePrefab,
           _projectileSpawnPoint.position,
           transform.rotation);
        clone.GetComponent<PlayerProjectile>().Setup(targetPoint, _weaponSetting.Damage, _weaponSetting.AttackDistance);
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
