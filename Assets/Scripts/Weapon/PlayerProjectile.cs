using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float _projectileDistance = 0;  //최대발사 거리
    private int _damage = 0;

    private Rigidbody _rigidbody;

    [SerializeField]
    private float _moveSpeed = 20f;
    [SerializeField]
    private Vector3 _moveDirection = Vector3.zero;

    public void Setup(Vector3 position, int damage, float Distacne)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _damage = damage;
        _projectileDistance = Distacne;

        StartCoroutine("OnMove", position);
    }

    void Update()
    {
        Vector3 moveDir =  transform.position + _moveDirection * _moveSpeed * Time.deltaTime;

        _rigidbody.MovePosition(moveDir);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<EnemyBase>().TakeDamage(_damage);
        }
        else if (collision.transform.CompareTag("InteractionObject"))
        {
            collision.transform.GetComponent<InteractionObjectBase>().TakeDamage(_damage);
        }
    }
}
