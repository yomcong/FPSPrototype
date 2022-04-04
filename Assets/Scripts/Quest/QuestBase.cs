using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestEvent : UnityEngine.Events.UnityEvent<bool> { }

public abstract class QuestBase : MonoBehaviour
{
    [HideInInspector]
    public QuestEvent OnQuestEvent = new QuestEvent();

    public abstract void StartQuest();

    public abstract void ClearQuest();
}