using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFight : QuestBase, IDamageable
{
    [SerializeField]
    private GameObject _finalObject;
    [SerializeField]
    private GameObject _paticleObject;
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private GameObject[] _target;

    private void Awake()
    {
        gameObject.SetActive(false);
        _finalObject.SetActive(false);
    }
    
    public override void StartQuest()
    {
        gameObject.SetActive(true);
        ProgressToQuestText(_tutorialScenario);

    }
    public override void ClearQuest()
    {
        _tutorialUI.SetActive(true);

        OnScenarioEvent.Invoke(_tutorialScenarioParam.ClearTutorial);
        OnScenarioNameEvent.Invoke(_tutorialScenarioParam.ClearTutorialName);

        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        _finalObject.SetActive(true);
        _paticleObject.SetActive(false);
        _tutorialUI.SetActive(false);
        ObstacleWallDestory();
        StartCoroutine("StartFinalFight");
    }

    private IEnumerator StartFinalFight()
    {
        while (true)
        {
            for (int i = 0; i < _target.Length; ++i)
            {
                if (_target[i] != null)
                {
                    break;
                }

                if (i == _target.Length - 1)
                {
                    OnQuestEvent.Invoke(_tutorialScenario);
                }
            }
            yield return null;
        }
    }
}
