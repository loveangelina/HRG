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
        dragon.PrepareRun(); // scream ��� �� 
        Debug.Log("scream");
        playerController.Stop();
        yield return new WaitForSecondsRealtime(3.0f);

        // ī�޶� ��ȯ 

        // �� ����
        leftDoor.transform.localEulerAngles = new Vector3(0, -90, 0);
        rightDoor.transform.localEulerAngles = new Vector3(0, 90, 0);
        Debug.Log("door open");

        // �÷��̾� �޸��� ���� 
        playerController.StartRun();

        // �巡���� walk 
        dragon.StartChase();

        // walk �÷��̾� ������ �̵��ϴ� �ڵ� 
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
