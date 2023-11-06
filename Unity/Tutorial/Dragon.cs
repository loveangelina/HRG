using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
        animator.SetBool("isSleeping", true);
    }

    private void Update()
    {
    }

    public void Sleeping()
    {
        animator.SetBool("isSleeping", true);
    }

    public void NotSleeping()
    {
        animator.SetBool("isSleeping", false);
    }

    public void PrepareRun()
    {
        animator.SetBool("prepareRun", true);
    }

    public void StartChase()
    {
        animator.SetTrigger("startChase");

        
    }

    public void Die()
    {
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("Sleeping"))
        {
        }
        animator.SetTrigger("die");
    }
}
