using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningMonster : MonoBehaviour
{
    Rigidbody rb;
    public Animator animator;
    public bool stateChain = false;
    [SerializeField] float velocity = 10;
    [SerializeField] GameObject player;
    GameManager gameManager;
    [SerializeField] public int count;
    [SerializeField] bool firemode = false;

    [SerializeField] bool isHIIT = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        count = 0;
    }

    private void FixedUpdate()
    {
        int distance = (int)player.gameObject.transform.position.x - (int)gameObject.transform.position.x;

        if(!stateChain)
        {
            if(isHIIT)
            {
                HighRun(distance);
            }
            else
            {
                ModerateRun(distance);
            }
        }
    }

    private void ModerateRun(int distance)
    {
        if (count <= 0)
        {
            Run(distance, 30);
        }
        else if (count == 1)
        {
            Run(distance, 40);
        }
        else if (count == 2)
        {
            Run(distance, 50);
        }
        else if (count == 3)
        {
            Run(distance, 60);
        }
        else
        {
            Run(distance, 70);
        }
    }

    private void HighRun(int distance)
    {
        if (count <= 0)
        {
            Run(distance, 50);
        }
        else if (count == 1)
        {
            Run(distance, 40);
        }
        else if (count == 2)
        {
            Run(distance, 30);
        }
        else if (count == 3)
        {
            Run(distance, 20);
        }
        else
        {
            Run(distance, 10);
        }
    }

    void Run(int distance, int threshold)
    {
        // 플레이어와 몬스터의 거리가 50이 될 때까지 몬스터가 플레이어쪽으로 이동
        if(distance > threshold)
        {
            // 플레이어랑 비슷한 속도로 달려야함 
            rb.MovePosition(transform.position + Vector3.right * Time.deltaTime * velocity);
        }
    }

    public void Chain()
    {
        animator.SetBool("laserBeam", true);
    }

    public void Unchain()
    {
        animator.SetBool("laserBeam", false);
    }

    public void FireMode()
    {
        animator.SetBool("FireMode", false);
    }

    public void UnFireMode()
    {
        count = 0;
        animator.SetBool("FireMode", true);
    }
}
