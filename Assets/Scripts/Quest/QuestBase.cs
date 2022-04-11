using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestEvent : UnityEngine.Events.UnityEvent<TutorialScenario> { }

[System.Serializable]
public class ScenarioEvent : UnityEngine.Events.UnityEvent<string> { }

public abstract class QuestBase : MonoBehaviour
{
    [HideInInspector]
    public QuestEvent OnQuestEvent = new QuestEvent();
    [HideInInspector]
    public ScenarioEvent OnScenarioEvent = new ScenarioEvent();

    [SerializeField]
    protected TutorialScenario _tutorialScenario;

    protected TutorialScenarioParam _tutorialScenarioParam = new TutorialScenarioParam();

    public abstract void StartQuest();

    public abstract void ClearQuest();

    public void ProgressToQuestText(TutorialScenario tutorialScenario)
    {
        switch (tutorialScenario)
        {
            case TutorialScenario.MovementPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart1);
                break;
            case TutorialScenario.MovementPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart2);
                break;
            case TutorialScenario.AttackPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart1);
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
                break;
            case TutorialScenario.FindEnemyPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.FindEnemyPart1);
                break;
        }
    }

}