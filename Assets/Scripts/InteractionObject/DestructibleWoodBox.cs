using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWoodBox : InteractionObjectBase
{
    [Header("Destructible Barrel")]
    [SerializeField]
    private GameObject _destrctibleWoodPieces;

    private bool _isDestroyed = false;

    public override void TakeDamage(int damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0 && _isDestroyed == false)
        {
            _isDestroyed = true;

            Instantiate(_destrctibleWoodPieces, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
