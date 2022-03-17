using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRifle : WeaponBase
{
    //[Header("Fire Effects")]
    //[SerializeField]
    //private GameObject _muzzleFlashEffect;
    //
    //[Header("Spawn Points")]
    //[SerializeField]
    //private Transform _casingSpawnPoint;
    //[SerializeField]
    //private Transform _bulletSpawnPoint;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipTakeOutWeapon;
    [SerializeField]
    private AudioClip _audioClipFire;
    [SerializeField]
    private AudioClip _audioClipReload;

    [Header("Aim UI")]
    [SerializeField]
    private Image _imageAim;

    //private CasingMemoryPool casingMemoryPool;
    //private ImpactMemoryPool impactMemoryPool;
    private Camera _mainCamera;


    private void Awake()
    {
        Init();

        _mainCamera = Camera.main;

        _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        PlaySound(_audioClipTakeOutWeapon);

        //_muzzleFlashEffect.SetActive(false);

        onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

        ResetVariables();
    }

    public override void StartReload()
    {
        if (_isReload || _animator.IsAimMode == true /*|| weaponSetting.currentMagazine <= 0*/)
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
            if (_weaponSetting.isAutomaticAttack)
            {
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
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
        if (type == 0)
        {
            _isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    private void ResetVariables()
    {
        _isReload = false;
        _isAttack = false;
        _isAimModeChange = false;
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            yield return null;
        }
    }

    private void OnAttack()
    {
        if (Time.time - _lastAttackTime > _weaponSetting.attackRate)
        {
            if (_animator.MoveSpeed > 0.5f)
            {
                return;
            }

            if (_weaponSetting.currentAmmo <= 0)
            {
                return;
            }

            _lastAttackTime = Time.time;

            _weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

            //_animator.IsAimMode == true ? _animator.Play(_animator.AnimParam.AimFire, -1, 0) :
            //    _animator.Play(_animator.AnimParam.Fire, -1, 0);

            if(_animator.IsAimMode)
            {
                _animator.Play(_animator.AnimParam.AimFireHash, -1, 0);
            }
            else
            {
                _animator.Play(_animator.AnimParam.FireHash, -1, 0);
                StartCoroutine("OnMuzzleFlashEffect");
            }
            
            PlaySound(_audioClipFire);

            //casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            TwoStepRaycast();
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, _weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * _weaponSetting.attackDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * _weaponSetting.attackDistance, Color.red);

        //Vector3 attackDirection = (targetPoint - _bulletSpawnPoint.position).normalized;
        //if (Physics.Raycast(_bulletSpawnPoint.position, attackDirection, out hit, _weaponSetting.attackDistance))
        //{
        //    //impactMemoryPool.SpawnImpact(hit);
        //
        //    //if (hit.transform.CompareTag("ImpactEnemy"))
        //    //{
        //    //    hit.transform.GetComponent<EnemyFSM>().TakeDamage(weaponSetting.damage);
        //    //}
        //    //else if (hit.transform.CompareTag("InteractionObject"))
        //    //{
        //    //    hit.transform.GetComponent<InteractionObject>().TakeDamage(weaponSetting.damage);
        //    //}
        //
        //}
        //Debug.DrawRay(_bulletSpawnPoint.position, attackDirection * _weaponSetting.attackDistance, Color.blue);
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        //_muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(_weaponSetting.attackRate * 0.3f);

        //_muzzleFlashEffect.SetActive(false);
    }
    private IEnumerator OnReload()
    {
        _isReload = true;

        _animator.OnReload();
        PlaySound(_audioClipReload);

        while (true)
        {
            if (_audioSource.isPlaying == false && _animator.CurrentAnimationIs(_animator.AnimParam.Movement))
            {
                _isReload = false;

                _weaponSetting.currentAmmo = _weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(_weaponSetting.currentAmmo, _weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator OnAimModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        _animator.IsAimMode = !_animator.IsAimMode;
        //_imageAim.enabled = !_imageAim.enabled;

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
}
