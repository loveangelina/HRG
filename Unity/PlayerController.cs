using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    Animator animator;
    [SerializeField] RunningMonster monster;
    [SerializeField] GameObject player;
    [SerializeField] GameObject laserBeam;
    public bool isMoving = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Run();
        }

        //Vector3 start = new Vector3(monster.gameObject.transform.position.x + 15, 0, 0);
        //transform.position = Vector3.MoveTowards(transform.position, start, 0.5f);
    }

    void Run()
    {
        if(isMoving)
        {
            //if(gameManager.CheckRunningState())
            rb.MovePosition(transform.position + Vector3.right * Time.deltaTime);
        }
        else
        // ������ ��
        {
            //animator.SetBool("isStopping", true);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            //player.gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.gameObject.transform.position.x - 3, 0, 0), 0.01f);
            //laserBeam.SetActive(true);

            //gameManager.steps
            // �̰� ������ �����̳� 5�� ������ �޸���! �׷� �̼� �ִ°ɷ� �ٲٱ� 
        }
    }

    public void ChangeStateForPortal()
    {
        animator.SetTrigger("portal");
    }

    public void Chain()
    {
        laserBeam.SetActive(true);
        //rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        animator.SetBool("isStopping", true);
    }

    public void Unchain()
    {
        animator.SetBool("isStopping", false);
        //rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        //player.gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(player.gameObject.transform.position.x - 3, 0, 0), 0.01f);
        laserBeam.SetActive(false);
    }

}
