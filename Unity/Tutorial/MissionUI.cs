using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionUI : TutorialBase
{
    [SerializeField] GameObject missionImageUI;
    [SerializeField] GameObject missionText;

    TutorialController tutorialController;

    public override void Enter(TutorialController controller)
    {
        StartCoroutine(ShowMissionUI());
    }

    public override void Execute(TutorialController controller)
    {
        tutorialController = controller;
    }

    public override void Exit()
    {
        
    }

    IEnumerator ShowMissionUI()
    {
        missionImageUI.SetActive(true);
        missionText.SetActive(true);

        yield return new WaitForSecondsRealtime(4f);

        missionImageUI.SetActive(false);
        missionText.SetActive(false);

        tutorialController.SetNextTutorial();
    }
}
