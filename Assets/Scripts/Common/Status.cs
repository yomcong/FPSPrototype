using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent OnHPEvent = new HPEvent();

    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _runSpeed;

    [SerializeField]
    private int _maxHP = 100;
    private int _currentHP;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public bool DecreaseHP(int damage)
    {
        int previousHP = _currentHP;

        _currentHP = _currentHP - damage > 0 ? _currentHP - damage : 0;

        OnHPEvent.Invoke(previousHP, _currentHP);

        if (_currentHP == 0)
        {
            return true;
        }

        return false;
    }

    public void IncreaseHP(int hp)
    {
        int previousHP = _currentHP;

        _currentHP = _currentHP + hp > _maxHP ? _maxHP : _currentHP + hp;

        OnHPEvent.Invoke(previousHP, _currentHP);
    }
}
