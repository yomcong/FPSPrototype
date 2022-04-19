using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMovement : MonoBehaviour
{
    private float _projectileDistance = 30;  //최대발사 거리
    private int _damage = 5;

    [SerializeField]
    private float _moveSpeed = 20f;
    [SerializeField]
    private Vector3 _moveDirection = Vector3.zero;

    public void Setup(Vector3 position)
    {
        Vector3 targetPosition = new Vector3(position.x + Random.Range(-0.2f, 0.2f), 
            position.y + Random.Range(-0.2f, 0.2f), 
            position.z);
        
        StartCoroutine("OnMove", targetPosition);
    }

    void Update()
    {
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
    }

    private void MoveTo(Vector3 direction)
    {
        _moveDirection = direction;
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 start = transform.position;

        MoveTo((targetPosition - transform.position).normalized);

        while (true)
        {
            if (Vector3.Distance(transform.position, start) >= _projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }

            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
