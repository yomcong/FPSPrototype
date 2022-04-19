using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestEvent : UnityEngine.Events.UnityEvent<TutorialScenario> { }

[System.Serializable]
public class ScenarioEvent : UnityEngine.Events.UnityEvent<string> { }
[System.Serializable]
public class ScenarioNameEvent : UnityEngine.Events.UnityEvent<string> { }

public abstract class QuestBase : MonoBehaviour
{
    [HideInInspector]
    public QuestEvent OnQuestEvent = new QuestEvent();
    [HideInInspector]
    public ScenarioEvent OnScenarioEvent = new ScenarioEvent();
    [HideInInspector]
    public ScenarioNameEvent OnScenarioNameEvent = new ScenarioNameEvent();

    [SerializeField]
    protected TutorialScenario _tutorialScenario;
    [SerializeField]
    protected GameObject[] _tempObstacleWall;

    protected TutorialScenarioParam _tutorialScenarioParam = new TutorialScenarioParam();

    public abstract void StartQuest();

    public abstract void ClearQuest();

    public void ObstacleWallDestory()
    {
        for (int i = 0; i < _tempObstacleWall.Length; ++i)
        {
            _tempObstacleWall[i].GetComponent<RoadBlockObject>()?.IsDisable();
            _tempObstacleWall[i].gameObject.SetActive(false);
        }
    }

    public void ProgressToQuestText(TutorialScenario tutorialScenario)
    {
        switch (tutorialScenario)
        {
            case TutorialScenario.MovementPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart1);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.MovementName);
                break;
            case TutorialScenario.MovementPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart2);
                break;
            case TutorialScenario.AttackPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart1);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.AttackName);
                break;
            case TutorialScenario.AttackPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart2);
                break;
            case TutorialScenario.AttackPart3:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart3);
                break;
            case TutorialScenario.AttackPart4:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart4);
                break;
            case TutorialScenario.ItemAndObject:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart1);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.ObjectName);
                break;
            case TutorialScenario.FindEnemyPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.FindEnemyPart1);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.EnemyName);
                break;
            case TutorialScenario.FindEnemyPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.FindEnemyPart2);
                break;
            case TutorialScenario.ObstacleObject:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.ObstacleObject);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.ObstacleName);
                break;
            case TutorialScenario.FinalFight:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.FinalFight);
                OnScenarioNameEvent.Invoke(_tutorialScenarioParam.FinalFightName);
                break;
        }

        
    }

}