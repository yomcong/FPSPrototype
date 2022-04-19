using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _destroyParticle;
    public void IsDisable()
    {
        Instantiate(_destroyParticle, transform.position, transform.rotation);
    }
}
