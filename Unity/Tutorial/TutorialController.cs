using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private List<TutorialBase> tutorials;
	[SerializeField]
	private string nextSceneName = "";

	private TutorialBase currentTutorial = null;
	private int currentIndex = -1;

	private void Start()
	{
		SetNextTutorial();
	}

	private void Update()
	{
		if (currentTutorial != null)
		{
			currentTutorial.Execute(this);
		}

		// 씬 넘어갈때 안달리는 경우 치팅방지 
	}

	public void SetNextTutorial(int index = -1)
	{
		if(index != -1)
        {
			// 31로 이동 
			currentIndex = index;
		}

		// 현재 튜토리얼의 Exit() 메소드 호출
		if (currentTutorial != null)
		{
			currentTutorial.Exit();
		}

		// 마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 메소드 호출
		if (currentIndex >= tutorials.Count - 1)
		{
			CompletedAllTutorials();
			return;
		}

		// 다음 튜토리얼 과정을 currentTutorial로 등록
		currentIndex++;
		currentTutorial = tutorials[currentIndex];

		// 새로 바뀐 튜토리얼의 Enter() 메소드 호출
		currentTutorial.Enter(this);
		Debug.Log("currentIndex : " + currentIndex);
	}

	public void CompletedAllTutorials()
	{
		currentTutorial = null;

		// 행동 양식이 여러 종류가 되었을 때 코드 추가 작성
		// 현재는 씬 전환

		if (!nextSceneName.Equals(""))
		{
			SceneManager.LoadScene(nextSceneName);
			
		}
	}
}
