using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private int dir = 1;

    void Update()
    {
        transform.position += new Vector3(_speed * Time.deltaTime * dir, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        dir *= -1;
    }
}
