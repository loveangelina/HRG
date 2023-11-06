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

        // �÷��̾� ���� 
        playerController.IsMoved = false;
		playerController.IsWalking = false;
		playerController.animator.SetBool("foundDoor", true);

		// ī�޶� ��ȯ
		silentWalkCamera.SetActive(false);
		openDoorViewCamera.SetActive(true);

		if (!isEnd && GameManager.time[GameManager.indexOfSection] > 4 * 60)
		{
			isEnd = true;
			// �÷��̾� 104�� �̵� 
			playerController.MoveForEnd();

			// 32_dooropen���� �̵� 
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
		// ���� �����ְ�

		yield return new WaitForSecondsRealtime(5f);

		// �� ������ 
		leftDoor.transform.localEulerAngles = new Vector3(0, -90, 0);
		rightDoor.transform.localEulerAngles = new Vector3(0, 90, 0);

		// �ٽ� �÷��̾� �̵� 
		playerController.animator.SetBool("foundDoor", false);
		playerController.IsMoved = true;
		playerController.IsWalking = true;

		// 2�� ���ٰ� �̼�â �� 
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
