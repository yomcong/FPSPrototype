using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObjectBase : MonoBehaviour, IDamageable
{
    [Header("Interaction Object")]
    [SerializeField]
    protected int _maxHP = 100;
    protected int _currentHP;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public abstract void TakeDamage(int damage);
}
