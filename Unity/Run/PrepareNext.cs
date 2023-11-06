using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareNext : TutorialBase
{
    GameManager gameManager;

    public override void Enter(TutorialController controller)
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.IncreaseIndexOfSection();
        Debug.Log("IncreaseIndexOfSection : " + GameManager.indexOfSection);

        controller.SetNextTutorial();
        
    }

    public override void Execute(TutorialController controller)
    {
        
    }

    public override void Exit()
    {
        
    }
}
