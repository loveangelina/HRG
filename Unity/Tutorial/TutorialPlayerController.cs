using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
	private Rigidbody rb;
	public Animator animator;

	public bool IsMoved { set; get; } = false;  // 게임 잠시 멈춤 여부
	public bool IsWalking { set; get; } = false;   // 플레이어 이동 여부

	private void Start()
	{
		IsMoved = false;
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	public void Walking()
    {
		animator.SetBool("isSilentWalking", true);
		//rb.MovePosition(transform.position + Vector3.right * Time.deltaTime);
	}

	public void NotWalking()
    {
		animator.SetBool("isSilentWalking", false);
	}

	public void Stop()
    {
		animator.SetTrigger("prepareRun");
	}

	public void StartRun()
    {
		animator.SetTrigger("startRun");
	}

	public void Victory()
    {
		while(!animator.GetCurrentAnimatorStateInfo(0).IsName("SilentWalk_Walk@loop"))
        {

        }
		GetComponent<Transform>().transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		animator.SetTrigger("end");
    }

	public void MoveForEnd()
    {
		GetComponent<Transform>().position = new Vector3(103, 0, 0);
    }
}
