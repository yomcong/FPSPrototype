using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttack : QuestBase
{
    [SerializeField]
    private GameObject[] _tempObstacleWall;
    [SerializeField]
    private GameObject[] _target;
    private bool[] _targetHitCheck;
    private void Awake()
    {
        gameObject.SetActive(false);

        _targetHitCheck = new bool[_target.Length];

        for (int i = 0; i < _target.Length; ++i)
        {
            _targetHitCheck[i] = false;
            _target[i].GetComponent<TutorialTargetEvent>().OnTargetEnevt.AddListener(TargetHitCheck);
        }
    }

    public override void StartQuest()
    {
        gameObject.SetActive(true);
    }

    public override void ClearQuest()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _tempObstacleWall.Length; ++i)
        {
            _tempObstacleWall[i].gameObject.SetActive(false);
        }
    }

    private void TargetHitCheck(GameObject gameObject)
    {
        for (int i = 0; i < _target.Length; ++i)
        {
            if(_target[i] == gameObject)
            {
                _targetHitCheck[i] = true;
                break;
            }
        }

        for (int i = 0; i < _targetHitCheck.Length; ++i)
        {
            if(_targetHitCheck[i] == false)
            {
                return;
            }
        }

        OnQuestEvent.Invoke(_tutorialScenario);
    }

}
