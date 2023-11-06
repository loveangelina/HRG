using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArrowBlink : MonoBehaviour
{
	[SerializeField]
	private float fadeTime; // ���̵� �Ǵ� �ð�
	private Image fadeImage;    // ���̵� ȿ���� ���Ǵ� Image UI

	private void Awake()
	{
		fadeImage = GetComponent<Image>();
	}

	private void OnEnable()
	{
		// Fade ȿ���� In -> Out ���� �ݺ��Ѵ�.
		StartCoroutine(FadeInOut());
	}

	private IEnumerator FadeInOut()
	{
		while (true)
		{
			yield return StartCoroutine(Fade(0.8f, 0));    // Fade In

			yield return StartCoroutine(Fade(0, 0.8f));    // Fade Out
		}
	}

	private IEnumerator Fade(float start, float end)
	{
		float current = 0;
		float percent = 0;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / fadeTime;

			Color color = fadeImage.color;
			color.a = Mathf.Lerp(start, end, percent);
			fadeImage.color = color;

			yield return null;
		}
	}
}
