using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmmoPack : MonoBehaviour, IItemBase
{
    [SerializeField]
    private GameObject _ammoEffectPrefab;
    [SerializeField]
    private int _increaseAmmo = 2;
    [SerializeField]
    private float _rotateSpeed = 50;

    private IEnumerator Start()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void Use(GameObject entity)
    {
        entity.GetComponent<WeaponSwitching>().IncreaseAllAmmo();

        Instantiate(_ammoEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
