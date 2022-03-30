using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private PlayerHUD _playerHUD;

    [SerializeField]
    private WeaponBase[] weapons;

    private WeaponBase _currentWeapon;
    private WeaponBase _previouseWeapon;

    private void Awake()
    {
        _playerHUD.SetupAllWeapons(weapons);

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }

        }

        SwitchingWeapon(WeaponType.Main);

    }

    private void Update()
    {
        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }

        int inputIndex = 0;
        if (int.TryParse(Input.inputString, out inputIndex) && (inputIndex > 0 && inputIndex < 5))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }
    }

    private void SwitchingWeapon(WeaponType weaponType)
    {
        if (weapons[(int)weaponType] == null)
        {
            return;
        }

        if (_currentWeapon != null)
        {
            _previouseWeapon = _currentWeapon;
        }

        _currentWeapon = weapons[(int)weaponType];

        if (_currentWeapon == _previouseWeapon)
        {
            return;
        }

        _playerController.SwitchingWeapon(_currentWeapon);
        _playerHUD.SwitchingWeapon(_currentWeapon);

        if (_previouseWeapon != null)
        {
            _previouseWeapon.gameObject.SetActive(false);
        }

        _currentWeapon.gameObject.SetActive(true);
    }

    public void IncreaseAmmo()
    {
        _currentWeapon.IncreaseAmmo();
    }

    public void IncreaseAllAmmo()
    {
        for(int i=0; i<weapons.Length; ++i)
        {
            if (weapons[i] != null)
            {
                weapons[i].IncreaseAmmo();
            }
            _currentWeapon.OnAmmoEvent.Invoke(_currentWeapon.CurrentAmmo, _currentWeapon.MaxAmmo);
        }
    }
}
