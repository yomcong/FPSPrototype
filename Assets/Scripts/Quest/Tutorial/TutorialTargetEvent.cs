using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TargetEnevt : UnityEngine.Events.UnityEvent<GameObject> { };

public class TutorialTargetEvent : MonoBehaviour
{
    private bool _onHit = false;

    [HideInInspector]
    public TargetEnevt OnTargetEnevt = new TargetEnevt();

    public void OnHitEvent()
    {
        if (_onHit == false)
        {
            _onHit = true;

            OnTargetEnevt.Invoke(gameObject);
        }
    }

}
