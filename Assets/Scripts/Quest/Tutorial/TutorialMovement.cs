using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialMovement : QuestBase
{
    private CapsuleCollider _capsuleCollider;
    [SerializeField]
    private GameObject[] _tempObstacleWall;
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        gameObject.SetActive(false);
    }

    public override void StartQuest()
    {
        gameObject.SetActive(true);
        ProgressToQuestText(_tutorialScenario);
    }

    public override void ClearQuest()
    {
        gameObject.SetActive(false);
        for(int i=0; i< _tempObstacleWall.Length; ++i)
        {
            _tempObstacleWall[i].gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnQuestEvent.Invoke(_tutorialScenario);
        }
    }
}
