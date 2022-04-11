using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemAndObject : QuestBase
{
    [SerializeField]
    private GameObject[] _tempObstacleWall;
    [SerializeField]
    private GameObject[] _interactionObject;
    [SerializeField]
    private GameObject[] _itemObject;

    private void Awake()
    {
        transform.gameObject.SetActive(false);

        for (int i = 0; i < _interactionObject.Length; ++i)
        {
            _interactionObject[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _itemObject.Length; ++i)
        {
            _itemObject[i].gameObject.SetActive(false);
        }

    }

    public override void StartQuest()
    {
        gameObject.SetActive(true);
        ProgressToQuestText(_tutorialScenario);
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine("InteractToBarrel"); 
        }
        else
        {
            gameObject.SetActive(true);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine("InteractToBarrel");
            }
        }
    }

    public override void ClearQuest()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _tempObstacleWall.Length; ++i)
        {
            _tempObstacleWall[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator InteractToBarrel()
    {
        for (int i = 0; i < _interactionObject.Length; ++i)
        {
            _interactionObject[i].gameObject.SetActive(true);
        }

        while (true)
        {
            for (int i = 0; i < _interactionObject.Length; ++i)
            {
                if(_interactionObject[i] != null)
                {
                    break;
                }

                if( i == _interactionObject.Length - 1)
                {
                    StartCoroutine("InteractToItem");
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart2);
                    yield break;
                }
            }

            yield return null;

        }
    }

    private IEnumerator InteractToItem()
    {
        for (int i = 0; i < _itemObject.Length; ++i)
        {
            _itemObject[i].gameObject.SetActive(true);
        }

        while (true)
        {
            for (int i = 0; i < _itemObject.Length; ++i)
            {
                if (_itemObject[i] != null)
                {
                    break;
                }

                if (_itemObject[0] == null)
                {
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart3);
                }

                if (i == _itemObject.Length - 1)
                {
                    OnQuestEvent.Invoke(_tutorialScenario);
                    yield break;
                }
            }

            yield return null;

        }

    }

}
