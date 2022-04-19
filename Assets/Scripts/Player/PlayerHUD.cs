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
    private QuestBase _quest;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI _textWeaponName;
    [SerializeField]
    private Image _imageAutomaticIcon;
    [SerializeField]
    private Image _imageSingleIcon;
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

    [Header("Scenario")]
    [SerializeField]
    private TextMeshProUGUI _textScenario;
    [SerializeField]
    private TextMeshProUGUI _textScenarioName;

    private List<GameObject> magazineList;

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private Image imageBloodScreen;
    [SerializeField]
    private AnimationCurve curveBloodScreen;


    private void Awake()
    {
        _status.OnHPEvent.AddListener(UpdateHPHUD);

        _imageAutomaticIcon.gameObject.SetActive(false);
        _imageSingleIcon.gameObject.SetActive(true);
    }

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].OnAmmoEvent.AddListener(UpdateAmmoHUD);
            weapons[i].OnAutomaticEvent.AddListener(UpdateAutomaticFireIcon);
            weapons[i].OnGrenadeEvent.AddListener(UpdateGrenadeHUD);
        }
    }

    public void SetupAllQuests(QuestBase[] quests)
    {
        for (int i = 0; i < quests.Length; ++i)
        {
            quests[i].OnScenarioEvent.AddListener(UpdateScenario);
            quests[i].OnScenarioNameEvent.AddListener(UpdateScenarioName);
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

    private void UpdateGrenadeHUD(int currentGrenade)
    {
        _textGrenadeAmmo.text = $"{currentGrenade}";
    }
    

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
        textHP.text = "HP " + current;

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

    private void UpdateAutomaticFireIcon(bool isAutomatic)
    {
        _imageAutomaticIcon.gameObject.SetActive(isAutomatic);
        _imageSingleIcon.gameObject.SetActive(isAutomatic == false);
    }

    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }

    private void UpdateScenario(string scenario)
    {
        _textScenario.text = scenario;
    }

    private void UpdateScenarioName(string scenarioName)
    {
        _textScenarioName.text = scenarioName;
    }
}
