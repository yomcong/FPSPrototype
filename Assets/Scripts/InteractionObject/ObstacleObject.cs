using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : InteractionObjectBase
{
    bool _inCover = false;

    public bool InCover
    {
        set => _inCover = value;
        get => _inCover;
    }

    public override void TakeDamage(int damage)
    {

    }
}
