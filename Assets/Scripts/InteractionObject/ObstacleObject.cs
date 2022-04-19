using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : InteractionObjectBase
{
    bool _inCover = false;

    [SerializeField]
    private GameObject _destroyParticle;

    public bool InCover
    {
        set => _inCover = value;
        get => _inCover;
    }

    public override void TakeDamage(int damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0 )
        {
            Instantiate(_destroyParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
