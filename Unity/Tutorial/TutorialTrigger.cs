using System.Collections;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
	[SerializeField]
	private TutorialPlayerController playerController;
	[SerializeField]
	private Dragon dragon;               // dragon
	// [SerializeField] private Transform triggerObject;
	[SerializeField] GameObject player;
	[SerializeField] int goalPosition = 0;
	[SerializeField] int count = 0;

	public bool isTrigger { set; get; } = false;    // ���� ��ǥ ���� 

	GameManager gameManager;

	Coroutine coroutine;

	public override void Enter(TutorialController controller)
	{
		//playerController.SetPlayerPosition();
		//dragon.Sleeping();
		//dragon.isSleeping = true;
		// �÷��̾� �̵� ����
		playerController.IsMoved = true;
		playerController.Walking();

		// Trigger ������Ʈ Ȱ��ȭ
		//triggerObject.gameObject.SetActive(true);

		gameManager = FindObjectOfType<GameManager>();
		coroutine = StartCoroutine(CheckCount());
	}

	public override void Execute(TutorialController controller)
	{
		// ��ǥ ���� �����ϸ�
		if (player.transform.position.x > goalPosition)
        {
			if(playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("SilentWalk_Walk@loop"))
            {
				controller.SetNextTutorial();
			}

			
		}

		// �ɹڼ��� �ѹ� 100 �̻��϶��� �巡���� ���ٰ� �ٽ� 3�� �Ŀ� ���� �÷��̾�� �ٽ� ������ ������ 
		// �ٵ� ���� �ڵ�� 100 �̻��� ��ӵǸ� ������ �Ȱ����� �巡�ﵵ ��� ��ȿ�� 
		// Start�� ��ӵ��� �ڷ�ƾ �ְų� �׷����ҵ�

		
		// ���� ��� ���� üũ�ؼ�
		// ���踦 ������ 
		// ���� ����
		//if (isTrigger == true) 
		//{
		//	//playerController.IsMoved = false;
		//	controller.SetNextTutorial();
		//	Debug.Log("���� Ʃ�丮��");
		//}
	}

	public override void Exit()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}

	IEnumerator CheckCount()
    {
		while(goalPosition < 160)
        {
			yield return new WaitForSecondsRealtime(1f);
			//if (playerController.heartRate > 100)
			if (gameManager.heartRate > GameManager.moderateIntensityHeartRate)
			{
				playerController.NotWalking();
				//playerController.IsWalking = false;
				//yield return new WaitUntil(() => playerController.IsWalking == false);
				//playerController.IsWalking = false;
				//dragon.isSleeping = false;
				dragon.NotSleeping();

				count++;
				GameManager.score -= 100;
				Debug.Log(count);



                while (!playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("KnockdownToBack_ToStandC"))
                {
                    yield return null;
                }

				playerController.Walking();
				//playerController.IsWalking = true;
				//dragon.isSleeping = true;
				dragon.Sleeping();
				//yield return new WaitUntil(() => playerController.IsWalking == true);
				yield return new WaitForSecondsRealtime(5f);

            }
            else
            {
				playerController.Walking();
				//playerController.IsWalking = true;
				//dragon.isSleeping = true;
				dragon.Sleeping();
			}
		}
	}
}


