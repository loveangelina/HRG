using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareEnd : TutorialBase
{
    [SerializeField] TutorialPlayerController playerController;
    [SerializeField] Dragon dragon;

    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject playerViewCamera;

    public override void Enter(TutorialController controller)
    {
        // ���� ��Ŭ ��Ƽ�� 
        // �÷��̾ �巡�� ���� �ٶ󺸰� ����� 

        // 
        //StopCoroutine("CheckCount");
        Debug.Log("prepare end enter");
        StartCoroutine(DieDragon());
    }

    public override void Execute(TutorialController controller)
    {

    }

    public override void Exit()
    {

    }

    IEnumerator DieDragon()
    {
        // ī�޶� ��ȯ
        mainCamera.SetActive(false);
        playerViewCamera.SetActive(true);

        //�巡�� �״� ���
        dragon.Die();

        // �÷��̾ ������ ����
        // �¸����� 
        playerController.Victory();

        

        yield return null;
        // ���� yield return���� �÷��̾ ������ ���� 
        // �¸� �����ϴ� �ڷ�ƾ ���� 

        // �� ������ setnext�ؼ� ���� ������ �Ѿ�� 
        // � �󸶳� �߰� ���� �������� �߰� 
    }
}
