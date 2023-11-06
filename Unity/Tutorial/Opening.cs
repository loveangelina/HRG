using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : TutorialBase
{
    [SerializeField] GameObject[] leftDoors;
    [SerializeField] GameObject[] rightDoors;

    [SerializeField] GameObject openingCamera;
    [SerializeField] GameObject playerFaceCamera;
    [SerializeField] GameObject mainCamera;

    [SerializeField] TutorialPlayerController playerController;

    public override void Enter(TutorialController controller)
    {
        openingCamera.SetActive(true);
        mainCamera.SetActive(false);

        int index = 0;
        while (index < leftDoors.Length)
        {
            leftDoors[index].transform.localEulerAngles = new Vector3(0, -90, 0);
            rightDoors[index].transform.localEulerAngles = new Vector3(0, 90, 0);
            index++;
        }


        StartCoroutine(ChangeCamera(controller));

    }

    public override void Execute(TutorialController controller)
    {
    }

    public override void Exit()
    {
        
    }

    IEnumerator ChangeCamera(TutorialController controller)
    {
        yield return new WaitForSecondsRealtime(2);

        // ¹® Â÷·Ê´ë·Î ´Ý±â
        int index = leftDoors.Length - 1;
        while (index >= 0)
        {
            leftDoors[index].transform.localEulerAngles = new Vector3(0, 0, 0);
            rightDoors[index].transform.localEulerAngles = new Vector3(0, 0, 0);
            index--;
            yield return new WaitForSecondsRealtime(1f);
        }

        openingCamera.SetActive(false);
        playerFaceCamera.SetActive(true);
        playerController.animator.SetBool("startOpening", true);

        yield return new WaitForSecondsRealtime(2f);

        playerFaceCamera.SetActive(false);
        mainCamera.SetActive(true);
        playerController.animator.SetBool("startOpening", false);

        yield return new WaitForSecondsRealtime(1f);

        controller.SetNextTutorial();
    }

}
