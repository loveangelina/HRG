using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareRun : TutorialBase
{
    [SerializeField] TutorialPlayerController playerController;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dragonGameObject;
    [SerializeField] Dragon dragon;
    [SerializeField] Rigidbody rb;

    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject prepareRunCamera;

    public override void Enter(TutorialController controller)
    {
        mainCamera.SetActive(false);
        prepareRunCamera.SetActive(true);
        StartCoroutine(Chase(controller));
    }

    public override void Execute(TutorialController controller)
    {
        
    }

    public override void Exit()
    {
        GameManager.Instance.IncreaseIndexOfSection();
    }

    IEnumerator Chase(TutorialController controller)
    {
        dragon.PrepareRun(); // scream 계속 함 
        Debug.Log("scream");
        playerController.Stop();
        yield return new WaitForSecondsRealtime(3.0f);

        // 카메라 전환 

        // 문 열림
        leftDoor.transform.localEulerAngles = new Vector3(0, -90, 0);
        rightDoor.transform.localEulerAngles = new Vector3(0, 90, 0);
        Debug.Log("door open");

        // 플레이어 달리기 시작 
        playerController.StartRun();

        // 드래곤이 walk 
        dragon.StartChase();

        // walk 플레이어 쪽으로 이동하는 코드 
        float rotation = 180;
        while (dragon.transform.position.x < 130)
        {
            Vector3 targetPosition = player.transform.position - dragonGameObject.transform.position + new Vector3(0, 0, 4);
            if(rotation >= 110)
                rotation -= 90 * Time.deltaTime * 0.5f;
            dragonGameObject.transform.eulerAngles = new Vector3(0, rotation, 0);
            //dragonGameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.EulerAngles(new Vector3(0, 90, 0)), 0.2f);
            rb.MovePosition(dragonGameObject.transform.position + targetPosition * Time.deltaTime * 2);
            yield return null;
        }
        Debug.Log("next stage");

        controller.SetNextTutorial();
    }
}
