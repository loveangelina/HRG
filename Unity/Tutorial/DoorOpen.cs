using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : TutorialBase
{
	[SerializeField] TutorialPlayerController playerController;

	[SerializeField] GameObject silentWalkCamera;
	[SerializeField] GameObject openDoorViewCamera;

	[SerializeField] GameObject leftDoor;
	[SerializeField] GameObject rightDoor;

	[SerializeField] GameObject missionImageUI;
	[SerializeField] GameObject missionText;

	static bool isEnd = false;

	public override void Enter(TutorialController controller)
    {
        Debug.Log("door open enter");

        // 플레이어 멈춤 
        playerController.IsMoved = false;
		playerController.IsWalking = false;
		playerController.animator.SetBool("foundDoor", true);

		// 카메라 전환
		silentWalkCamera.SetActive(false);
		openDoorViewCamera.SetActive(true);

		if (!isEnd && GameManager.time[GameManager.indexOfSection] > 4 * 60)
		{
			isEnd = true;
			// 플레이어 104로 이동 
			playerController.MoveForEnd();

			// 32_dooropen으로 이동 
			controller.SetNextTutorial(8);

			return;
		}

		StartCoroutine(OpenDoor(controller));
	}

    public override void Execute(TutorialController controller)
    {
        
    }

    public override void Exit()
    {
        
    }

    IEnumerator OpenDoor(TutorialController controller)
	{
		// 열쇠 보여주고

		yield return new WaitForSecondsRealtime(5f);

		// 문 열리고 
		leftDoor.transform.localEulerAngles = new Vector3(0, -90, 0);
		rightDoor.transform.localEulerAngles = new Vector3(0, 90, 0);

		// 다시 플레이어 이동 
		playerController.animator.SetBool("foundDoor", false);
		playerController.IsMoved = true;
		playerController.IsWalking = true;

		// 2초 가다가 미션창 뜸 
		yield return new WaitForSecondsRealtime(5f);
		missionImageUI.SetActive(true);
		missionText.SetActive(true);

		yield return new WaitForSecondsRealtime(6f);
		missionImageUI.SetActive(false);
		missionText.SetActive(false);

		silentWalkCamera.SetActive(true);
		openDoorViewCamera.SetActive(false);

		controller.SetNextTutorial();
	}
}
