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
        // 매직 서클 액티브 
        // 플레이어가 드래곤 쪽을 바라보게 만들기 

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
        // 카메라 전환
        mainCamera.SetActive(false);
        playerViewCamera.SetActive(true);

        //드래곤 죽는 모션
        dragon.Die();

        // 플레이어가 정면을 보고
        // 승리포즈 
        playerController.Victory();

        

        yield return null;
        // 종료 yield return에서 플레이어가 정면을 보고 
        // 승리 포즈하는 코루틴 실행 

        // 그 다음에 setnext해서 다음 씬으로 넘어가서 
        // 운동 얼마나 했고 점수 몇점인지 뜨게 
    }
}
