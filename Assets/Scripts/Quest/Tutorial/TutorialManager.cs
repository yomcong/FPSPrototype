using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private QuestBase[] _questList;

    private int _currQuestIndex;

    private void Awake()
    {
        for (int i = 0; i < _questList.Length; ++i)
        {
            _questList[i].OnQuestEvent.AddListener(UpdateToQuest);
        }

        _currQuestIndex = 0;
    }

    private void Start()
    {
        _questList[_currQuestIndex].StartQuest();
    }

    private void UpdateToQuest(bool questSuccess)
    {
        if (questSuccess)
        {
            if (_currQuestIndex != _questList.Length - 1)
            {
                _currQuestIndex++;
                _questList[_currQuestIndex].StartQuest();
            }
        }
        else
        {
            //Àç½ÃÀÛ 
        }
    }
}
