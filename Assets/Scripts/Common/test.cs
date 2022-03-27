using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    float tesee;
    private void OnEnable()
    {
        tesee = 0.1f;

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        tesee += 0.1f;

        transform.localScale = new Vector3(tesee, tesee, tesee);
    }
}
