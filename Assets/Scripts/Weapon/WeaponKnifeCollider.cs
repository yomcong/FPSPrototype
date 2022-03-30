using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{
    [SerializeField]
    private ImpactMemoryPool _impactMemoryPool;

    private Collider _collider;
    private int _damage;

    private void Awake()
    {
        _impactMemoryPool = GetComponentInParent<ImpactMemoryPool>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        _damage = damage;
        _collider.enabled = true;

        StartCoroutine("DisablebyTime", 0.1f);
    }

    private IEnumerator DisablebyTime(float time)
    {
        yield return new WaitForSeconds(time);

        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _impactMemoryPool.SpawnImpact(other, transform);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<IDamageable>()?.TakeDamage(_damage);
        }
        else if (other.CompareTag("ObstacleObject"))
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);
        }
    }
}
