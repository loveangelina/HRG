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
        // �巡�� ����ġ
        // �÷��̾� ����ġ

        // �巡�� ��Ƽ�� 
        // �÷��̾� ��Ż���� �߰��ϴ� ��� (�ڿ� �� �ְ� ���� ���� ���� �ٸ��� �������..)

        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
            Debug.Log("�÷��̾� ��Ʈ�� null");

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
        // �÷��̾� ������ ���������� 
        //�ӹ� �����ϴ°� �������� ��Ʈ���� ���⼭ ������� 

        // 2�� �������� ���̾��� Ű�� 
        // ���� �� 5�� ���ܵ��� �� ���̾��� ���� 
    }

    public override void Exit()
    {
        
    }

    // �߰��� ���� 
    // �ɹڼ� ������ �� ������ count ������Ű�⸸ �ϰ� �ӹ��� �ȵ� 
    
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
            else    // �� �޸��� ������ 
            {
                successCount++;
                if(successCount > 3)    // ���� 4�� �� �޸��� �ν��� 
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


    // ���� ����
    // ���� �ȴ޸��ų� �޸��� ������ �ӹ� �ɸ� 
    IEnumerator CheckChain()
    {
        int successCount = 0;
        while (true)
        {
            if(gameManager.steps < 1)
            {
                // count ++ �϶����� ���� �߰��Ǵ°� ���߿�.. 
                dragon.count++;
                successCount = 0;
                Debug.Log("count : " + dragon.count);

                chain = true;
                //Debug.Log("�ӹ�");

                // �ӹ� ON
                dragon.stateChain = true;
                dragon.Chain();

                if (playerController != null)
                {
                    playerController.Chain();
                    playerController.isMoving = false;
                }

                // ���� �ȴ޸��� ��ٸ��� �ð� ���� �ȵ� 

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

                // �ӹ� ���� 
                //Debug.Log("����");

                dragon.stateChain = false;
                dragon.Unchain();

                playerController.isMoving = true;
                playerController.Unchain();

                // �ӹ� ���� �� ���� �ð� �ֱ� 
                yield return new WaitForSecondsRealtime(5);
            }
            // �� �޸��� �ִµ� �ɹڼ��� ����ġ ���ϸ� count++
            else if(gameManager.heartRate < GameManager.highIntensityHeartRate)
            {
                dragon.count++;
                successCount = 0;
                dragonAttack();
                Debug.Log("count : " + dragon.count);
                yield return new WaitForSecondsRealtime(10);
            }
            else    // �� �޸���
            {
                successCount++;
                if (successCount > 3)    // ���� 4�� �� �޸��� �ν��� 
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

    // �巡�� ���̾� 
    private void dragonAttack()
    {
        if(dragon.count > 4)
            dragon.animator.SetTrigger("attack");
    }
}
