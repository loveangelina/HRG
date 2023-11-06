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

	public bool isTrigger { set; get; } = false;    // 최종 목표 지점 

	GameManager gameManager;

	Coroutine coroutine;

	public override void Enter(TutorialController controller)
	{
		//playerController.SetPlayerPosition();
		//dragon.Sleeping();
		//dragon.isSleeping = true;
		// 플레이어 이동 가능
		playerController.IsMoved = true;
		playerController.Walking();

		// Trigger 오브젝트 활성화
		//triggerObject.gameObject.SetActive(true);

		gameManager = FindObjectOfType<GameManager>();
		coroutine = StartCoroutine(CheckCount());
	}

	public override void Execute(TutorialController controller)
	{
		// 목표 지점 도달하면
		if (player.transform.position.x > goalPosition)
        {
			if(playerController.animator.GetCurrentAnimatorStateInfo(0).IsName("SilentWalk_Walk@loop"))
            {
				controller.SetNextTutorial();
			}

			
		}

		// 심박수가 한번 100 이상일때만 드래곤이 깼다가 다시 3초 후에 잠들고 플레이어는 다시 앞으로 가야함 
		// 근데 지금 코드는 100 이상이 계속되면 앞으로 안가지고 드래곤도 계속 포효함 
		// Start에 계속도는 코루틴 넣거나 그래야할듯

		
		// 열쇠 얻는 조건 체크해서
		// 열쇠를 얻으면 
		// 문이 열림
		//if (isTrigger == true) 
		//{
		//	//playerController.IsMoved = false;
		//	controller.SetNextTutorial();
		//	Debug.Log("다음 튜토리얼");
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


