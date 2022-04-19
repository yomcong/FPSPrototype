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

        StartCoroutine("DisablebyTime");
    }

    private IEnumerator DisablebyTime()
    {
        yield return new WaitForSeconds(0.25f);

        _collider.enabled = true;

        yield return new WaitForSeconds(0.25f);

        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _impactMemoryPool.SpawnImpact(other, transform);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<IDamageable>()?.TakeDamage(_damage);
        }
        else if (other.CompareTag("InteractionObject"))
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);
        }
        else if (other.CompareTag("TriggerObject"))
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);
        }
    }
}
