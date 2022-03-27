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
    [SerializeField]
    private int _currentHP;

    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;

    //public float WalkSpeed
    //{
    //    get;
    //}
    //public float RunSpeed
    //{
    //    get;
    //}
    //public float CurrentHP
    //{
    //    get;
    //}
    //public float MaxHP
    //{
    //    get;
    //}

    //public bool DecreaseHP(int damage);
    //public void IncreaseHP(int hp);

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
