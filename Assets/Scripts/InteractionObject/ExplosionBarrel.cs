using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : InteractionObjectBase
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _destrctibleBarrelPieces;
    [SerializeField]
    private float _explosionDelayTime = 0.3f;
    [SerializeField]
    private float _explosionRadius = 10.0f;
    [SerializeField]
    private float _explosionForce = 1000.0f;
    [SerializeField]
    private int _explosionDamage = 45;
    [SerializeField]
    private GameObject _textDamage;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Transform _textDamageSpawnPoint;


    private bool _isExplode = false;

    public override void TakeDamage(int damage)
    {
        _currentHP -= damage;

        GameObject textDamage = Instantiate(_textDamage, _textDamageSpawnPoint.position, Quaternion.identity, _canvas.transform);
        textDamage.GetComponent<DamageText>()?.Setup(damage, _canvas, _textDamageSpawnPoint.position);

        if (_currentHP <= 0 && _isExplode == false)
        {
            StartCoroutine("ExplodeBarrel");
        }
    }

    private IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(_explosionDelayTime);

        _isExplode = true;

        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(_explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider hit in colliders)
        {
            if(hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(_explosionDamage);
                continue;
            }
            else if(hit.CompareTag("InteractionObject"))
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(300);

                Rigidbody tempRigid = hit.GetComponent<Rigidbody>();
                if(tempRigid != null)
                {
                    tempRigid.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
                }            
            }
        }

        Instantiate(_destrctibleBarrelPieces, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
