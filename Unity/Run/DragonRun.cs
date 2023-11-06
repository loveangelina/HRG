using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRun : TutorialBase
{
    PlayerController playerController;
    [SerializeField] RunningMonster dragon;
    [SerializeField] public bool chain = false;
    [SerializeField] Portal portal;
    [SerializeField] bool highIntensityMode = false;
    GameManager gameManager;

    public override void Enter(TutorialController controller)
    {
        // 드래곤 원위치
        // 플레이어 원위치

        // 드래곤 액티브 
        // 플레이어 포탈에서 뜨게하는 모션 (뒤에 문 있고 없고에 따라 맵을 다르게 만들던가..)

        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
            Debug.Log("플레이어 컨트롤 null");

        if (highIntensityMode)
        {
            StartCoroutine(CheckChain());
        }
        else
        {
            StartCoroutine(CheckCount());
        }

        StartCoroutine(portal.CheckPlayer(controller));
    }

    public override void Execute(TutorialController controller)
    {
        // 플레이어 걸음수 충족했을때 
        //속박 해제하는거 전반적인 컨트롤을 여기서 해줘야함 

        // 2분 지났을때 파이어모드 키고 
        // 포털 한 5초 남겨뒀을 때 파이어모드 끄기 
    }

    public override void Exit()
    {
        
    }

    // 중강도 구간 
    // 심박수 기준을 못 넘으면 count 증가시키기만 하고 속박은 안됨 
    
    IEnumerator CheckCount()
    {
        int successCount = 0;
        while(true)
        {
            if (gameManager.heartRate > GameManager.moderateIntensityHeartRate || gameManager.steps < 1)
            {
                dragon.count++;
                dragonAttack();
                successCount = 0;
            }
            else    // 잘 달리고 있으면 
            {
                successCount++;
                if(successCount > 3)    // 연속 4번 잘 달리면 부스터 
                {
                    dragon.count--;
                }
                else if(successCount > 10)
                {
                    dragon.count = 0;
                }
            }
            Debug.Log("successCount : " + successCount);
            yield return new WaitForSecondsRealtime(10);
        }
    }


    // 고강도 구간
    // 빨리 안달리거나 달리지 않으면 속박 걸림 
    IEnumerator CheckChain()
    {
        int successCount = 0;
        while (true)
        {
            if(gameManager.steps < 1)
            {
                // count ++ 일때마다 몬스터 추가되는건 나중에.. 
                dragon.count++;
                successCount = 0;
                Debug.Log("count : " + dragon.count);

                chain = true;
                //Debug.Log("속박");

                // 속박 ON
                dragon.stateChain = true;
                dragon.Chain();

                if (playerController != null)
                {
                    playerController.Chain();
                    playerController.isMoving = false;
                }

                // 빨리 안달리면 기다리는 시간 차감 안됨 

                int timer = 10;

                while(timer > 0)
                {
                    //Debug.Log(timer);
                    if (gameManager.steps > 3)
                    {
                        timer -= 2;
                        yield return new WaitForSecondsRealtime(1);
                        continue;
                    }
                    
                    timer--;
                    yield return new WaitForSecondsRealtime(1);
                }

                // 속박 해제 
                //Debug.Log("해제");

                dragon.stateChain = false;
                dragon.Unchain();

                playerController.isMoving = true;
                playerController.Unchain();

                // 속박 해제 후 조금 시간 주기 
                yield return new WaitForSecondsRealtime(5);
            }
            // 잘 달리고 있는데 심박수가 기준치 이하면 count++
            else if(gameManager.heartRate < GameManager.highIntensityHeartRate)
            {
                dragon.count++;
                successCount = 0;
                dragonAttack();
                Debug.Log("count : " + dragon.count);
                yield return new WaitForSecondsRealtime(10);
            }
            else    // 잘 달리면
            {
                successCount++;
                if (successCount > 3)    // 연속 4번 잘 달리면 부스터 
                {
                    dragon.count--;
                }
                else if (successCount > 10)
                {
                    dragon.count = 0;
                }
                Debug.Log("successCount : " + successCount);
                yield return new WaitForSecondsRealtime(10);
            }
            yield return new WaitForEndOfFrame();
        }

    }

    // 드래곤 파이어 
    private void dragonAttack()
    {
        if(dragon.count > 4)
            dragon.animator.SetTrigger("attack");
    }
}
