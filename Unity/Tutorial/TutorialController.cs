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

		// �� �Ѿ�� �ȴ޸��� ��� ġ�ù��� 
	}

	public void SetNextTutorial(int index = -1)
	{
		if(index != -1)
        {
			// 31�� �̵� 
			currentIndex = index;
		}

		// ���� Ʃ�丮���� Exit() �޼ҵ� ȣ��
		if (currentTutorial != null)
		{
			currentTutorial.Exit();
		}

		// ������ Ʃ�丮���� �����ߴٸ� CompletedAllTutorials() �޼ҵ� ȣ��
		if (currentIndex >= tutorials.Count - 1)
		{
			CompletedAllTutorials();
			return;
		}

		// ���� Ʃ�丮�� ������ currentTutorial�� ���
		currentIndex++;
		currentTutorial = tutorials[currentIndex];

		// ���� �ٲ� Ʃ�丮���� Enter() �޼ҵ� ȣ��
		currentTutorial.Enter(this);
		Debug.Log("currentIndex : " + currentIndex);
	}

	public void CompletedAllTutorials()
	{
		currentTutorial = null;

		// �ൿ ����� ���� ������ �Ǿ��� �� �ڵ� �߰� �ۼ�
		// ����� �� ��ȯ

		if (!nextSceneName.Equals(""))
		{
			SceneManager.LoadScene(nextSceneName);
			
		}
	}
}
