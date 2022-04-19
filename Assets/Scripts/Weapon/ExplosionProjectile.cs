using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject _explsionPrefab;
    [SerializeField]
    private float _explosionRadius = 10.0f;
    [SerializeField]
    private float _explosionForce = 500.0f;
    [SerializeField]
    private float _throwForce = 1000.0f;

    private int _explsionDamage;
    private new Rigidbody _rigidbody;

    public void Setup(float throwForce, int damage, Vector3 rotation)
    {
        _explsionDamage = damage;
        _throwForce = throwForce;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(rotation * _throwForce);

        Destroy(gameObject, 10f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explsionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explsionDamage / 5);
                continue;
            }
            else if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explsionDamage);
                continue;
            }
            else if (hit.CompareTag("InteractionObject"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explsionDamage);
            }
            else if( hit.CompareTag("ObstacleObject"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explsionDamage);
            }

        }

        Destroy(gameObject);
    }
}
