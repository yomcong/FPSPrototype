using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestEvent : UnityEngine.Events.UnityEvent<TutorialScenarioEvent> { }

[System.Serializable]
public class ScenarioEvent : UnityEngine.Events.UnityEvent<string> { }

public abstract class QuestBase : MonoBehaviour
{
    [HideInInspector]
    public QuestEvent OnQuestEvent = new QuestEvent();
    [HideInInspector]
    public ScenarioEvent OnScenarioEvent = new ScenarioEvent();

    [SerializeField]
    protected TutorialScenarioEvent _tutorialScenario;

    protected TutorialScenarioParam _tutorialScenarioParam = new TutorialScenarioParam();

    public abstract void StartQuest();

    public abstract void ClearQuest();

    public void ProgressToQuestText(TutorialScenarioEvent tutorialScenario)
    {
        switch (tutorialScenario)
        {
            case TutorialScenarioEvent.MovementPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart1);
                break;
            case TutorialScenarioEvent.MovementPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.MovementPart2);
                break;
            case TutorialScenarioEvent.AttackPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart1);
                break;
            case TutorialScenarioEvent.AttackPart2:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.AttackPart2);
                break;
            case TutorialScenarioEvent.ItemAndObject:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart1);
                break;
            case TutorialScenarioEvent.FindEnemyPart1:
                OnScenarioEvent.Invoke(_tutorialScenarioParam.FindEnemyPart1);
                break;

                
        }
    }

}