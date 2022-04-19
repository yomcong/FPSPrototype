using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    private EnemyStateBase _enemyState;
    public EnemyStateBase EnemyState
    {
        get => _enemyState;
    }

    public void Setup(EnemyStateBase defaultState)
    {
        _enemyState = defaultState;
    }

    private void Start()
    {
        _enemyState.StateEnter();
    }

    public void SetState(EnemyStateBase state)
    {
        if (_enemyState == state)
        {
            return;
        }
        else
        {
            _enemyState.StateExit();
            _enemyState = state;
            _enemyState.StateEnter();
        }
    }

    public void IsDie()
    {
        _enemyState.IsDie();
    }

}
