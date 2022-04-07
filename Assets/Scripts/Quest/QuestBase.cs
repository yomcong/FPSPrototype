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
}