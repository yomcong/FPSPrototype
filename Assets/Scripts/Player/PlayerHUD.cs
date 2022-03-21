using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Status _status;
    private WeaponBase _weapon;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI _textWeaponName;
    [SerializeField]
    private Image _imageWeaponIcon;
    [SerializeField]
    private Sprite[] _spriteWeaponIcons;
    [SerializeField]
    private Vector2[] _sizeWeaponIcons; 

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI _textAmmo;
    [SerializeField]
    private TextMeshProUGUI _textGrenadeAmmo;

    private List<GameObject> magazineList;

    //[Header("HP & BloodScreen UI")]
    //[SerializeField]
    //private TextMeshProUGUI textHP;
    //[SerializeField]
    //private Image imageBloodScreen;
    //[SerializeField]
    //private AnimationCurve curveBloodScreen;


    private void Awake()
    {
        _status.OnHPEvent.AddListener(UpdateHPHUD);
    }

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        //SetupMagazine();

        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].OnAmmoEvent.AddListener(UpdateAmmoHUD);
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        _weapon = newWeapon;

        SetupWeapons();
    }

    public void SetupWeapons()
    {
        _textWeaponName.text = _weapon.WeaponName.ToString();
        _imageWeaponIcon.sprite = _spriteWeaponIcons[(int)_weapon.WeaponName];
        _imageWeaponIcon.rectTransform.sizeDelta = _sizeWeaponIcons[(int)_weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        _textAmmo.text = $"<size=55>{ currentAmmo}/</size=40>{maxAmmo}";
    }

    //private void SetupMagazine()
    //{
    //    magazineList = new List<GameObject>();
    //    for (int i = 0; i < maxMagazineCount; ++i)
    //    {
    //        GameObject clone = Instantiate(magazineUIPrefab);
    //        clone.transform.SetParent(magazineParent);
    //        clone.SetActive(false);

    //        magazineList.Add(clone);
    //    }

    //    for (int i = 0; i < weapon.CurrentMagazine; ++i)
    //    {
    //        magazineList[i].SetActive(true);
    //    }

    //}

    private void UpdateMagazineHUD(int currentMagazine)
    {
        for (int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }

        for (int i = 0; i < currentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private void UpdateHPHUD(int previous, int current)
    {
        //textHP.text = "HP " + current;

        if (previous <= current)
        {
            return;
        }

        if (previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }
    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            //Color color = imageBloodScreen.color;
            //color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            //imageBloodScreen.color = color;

            yield return null;
        }
    }
}