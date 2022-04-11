using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialScenario { MovementPart1 = 0, MovementPart2, AttackPart1, AttackPart2
        , ItemAndObject, FindEnemyPart1};


public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private QuestBase[] _questList;
    [SerializeField]
    private PlayerHUD _playerHUD;


    private void Awake()
    {
        _playerHUD.SetupAllQuests(_questList);

        for (int i = 0; i < _questList.Length; ++i)
        {
            _questList[i].OnQuestEvent.AddListener(UpdateToQuest);
            _questList[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _questList[(int)TutorialScenario.MovementPart1].StartQuest();
    }

    private void UpdateToQuest(TutorialScenario questSuccess)
    {
        _questList[(int)questSuccess].ClearQuest();

        if(_questList.Length - 1 != (int)questSuccess )
        {
            _questList[(int)questSuccess + 1].gameObject.SetActive(true);
            _questList[(int)questSuccess + 1].StartQuest();
        }
    }
}
