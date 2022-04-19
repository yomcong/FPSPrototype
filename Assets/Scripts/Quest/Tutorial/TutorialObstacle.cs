using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObstacle : QuestBase
{
    [SerializeField]
    private GameObject[] _obstacleObject;
    private void Awake()
    {
        transform.gameObject.SetActive(false);

        for (int i = 0; i < _obstacleObject.Length; ++i)
        {
            _obstacleObject[i].gameObject.SetActive(false);
        }
    }

    public override void StartQuest()
    {
        gameObject.SetActive(true);
        ProgressToQuestText(_tutorialScenario);
        StartCoroutine("InteractToObstacle");
    }

    public override void ClearQuest()
    {
        gameObject.SetActive(false);

        ObstacleWallDestory();
    }

    public IEnumerator InteractToObstacle()
    {
        for (int i = 0; i < _obstacleObject.Length; ++i)
        {
            _obstacleObject[i].gameObject.SetActive(true);
        }

        while (true)
        {
            for (int i = 0; i < _obstacleObject.Length; ++i)
            {
                if (_obstacleObject[i] != null)
                {
                    break;
                }

                if (i == _obstacleObject.Length - 1)
                {
                    OnQuestEvent.Invoke(_tutorialScenario);
                    yield break;
                }
            }
            yield return null;

        }
    }
}
