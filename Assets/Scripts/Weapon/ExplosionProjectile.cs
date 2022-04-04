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

    public void Setup(int damage, Vector3 rotation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(rotation * _throwForce);

        _explsionDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explsionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explsionDamage);

                continue;
            }
            else if (hit.CompareTag("InteractionObject"))
            {
                hit.GetComponent<IDamageable>().TakeDamage(300);
            }
            else if( hit.CompareTag("ObstacleObject"))
            {
                Debug.Log("¾öÆó¹°");
            }

            //PlayerController player = hit.GetComponent<PlayerController>();
            //if (player != null)
            //{
            //    player.TakeDamage((int)(explsionDamage * 0.2f));
            //    continue;
            //}

            //EnemyFSM enemy = hit.GetComponentInParent<>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(explsionDamage);
            //    continue;
            //}

            //InteractionObject interaction = hit.GetComponent<InteractionObject>();
            //if (interaction != null)
            //{
            //    interaction.TakeDamage(explsionDamage);
            //}

            //Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            //if (rigidbody != null)
            //{
            //    rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            //}

        }

        Destroy(gameObject);
    }
}
