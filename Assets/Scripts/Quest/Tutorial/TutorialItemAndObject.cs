using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemAndObject : QuestBase
{
    //[SerializeField]
    //private GameObject[] _tempObstacleWall;
    [SerializeField]
    private GameObject[] _interactionObject;
    [SerializeField]
    private GameObject[] _healthObject;
    [SerializeField]
    private GameObject[] _ammoObject;

    private void Awake()
    {
        transform.gameObject.SetActive(false);

        for (int i = 0; i < _interactionObject.Length; ++i)
        {
            _interactionObject[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _healthObject.Length; ++i)
        {
            _healthObject[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _ammoObject.Length; ++i)
        {
            _ammoObject[i].gameObject.SetActive(false);
        }
    }

    public override void StartQuest()
    {
        gameObject.SetActive(true);
        ProgressToQuestText(_tutorialScenario);
        StartCoroutine("InteractToBarrel");
    }

    public override void ClearQuest()
    {
        gameObject.SetActive(false);

        ObstacleWallDestory();

        //for (int i = 0; i < _tempObstacleWall.Length; ++i)
        //{
        //    _tempObstacleWall[i].gameObject.SetActive(false);
        //}
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
                if (_interactionObject[i] != null)
                {
                    break;
                }

                if (i == _interactionObject.Length - 1)
                {
                    yield return new WaitForSeconds(1.5f);
                    StartCoroutine("InteractToHealthItem");
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart2);
                    OnScenarioNameEvent.Invoke(_tutorialScenarioParam.ItemName);

                    for (int j = 0; j < 4; ++j)
                    {
                        _tempObstacleWall[j].GetComponent<RoadBlockObject>()?.IsDisable();

                        _tempObstacleWall[j].gameObject.SetActive(false);
                    }

                    yield break;
                }
            }

            yield return null;

        }
    }

    private IEnumerator InteractToHealthItem()
    {
        for (int i = 0; i < _healthObject.Length; ++i)
        {
            _healthObject[i].gameObject.SetActive(true);
        }

        while (true)
        {
            for (int i = 0; i < _healthObject.Length; ++i)
            {
                if (_healthObject[i] != null)
                {
                    break;
                }

                if (_healthObject[0] == null)
                {
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart3);
                }

                if (i == _healthObject.Length - 1)
                {
                    StartCoroutine("InteractToAmmoItem");
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart4);
                    yield break;
                }
            }

            yield return null;

        }

    }

    private IEnumerator InteractToAmmoItem()
    {
        for (int i = 0; i < _ammoObject.Length; ++i)
        {
            _ammoObject[i].gameObject.SetActive(true);
        }

        while (true)
        {
            for (int i = 0; i < _ammoObject.Length; ++i)
            {
                if (_ammoObject[i] != null)
                {
                    break;
                }

                if (_ammoObject[0] == null && _ammoObject[1] == null)
                {
                    OnScenarioEvent.Invoke(_tutorialScenarioParam.ItemAndObjectPart5);
                }

                if (i == _ammoObject.Length - 1)
                {
                    OnQuestEvent.Invoke(_tutorialScenario);
                    yield break;
                }
            }

            yield return null;

        }
    }


}
