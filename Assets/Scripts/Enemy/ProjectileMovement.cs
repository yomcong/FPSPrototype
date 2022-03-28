using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    //private MovementTransform movement;
    private float _projectileDistance = 30;  //최대발사 거리
    private int damage = 5;
    

    [SerializeField]
    private float moveSpeed = 20f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public void Setup(Vector3 position)
    {
        //movement = GetComponent<MovementTransform>();
        float impactPoint = Random.Range(-0.2f, 0.2f);
        Vector3 temp = new Vector3(position.x + impactPoint, position.y + impactPoint, position.z);
        StartCoroutine("OnMove", temp);
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
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
            //Debug.Log("Player Hit");
            other.GetComponent<IDamageable>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
