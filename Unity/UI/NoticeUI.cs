using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    public GameObject subbox;
    public Text subintext;
    public Animator subAnimator;

    // coroutine delay
    private WaitForSeconds uiDelay1 = new WaitForSeconds(2.0f);
    private WaitForSeconds uiDelay2 = new WaitForSeconds(0.3f);

    private void Start()
    {
        subbox.SetActive(false);
    }

    public void ShowNotice(string message)
    {
        subintext.text = message;
        subbox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(SubDelay());
    }

    IEnumerator SubDelay()
    {
        subbox.SetActive(true);
        subAnimator.SetBool("isOn", true);  // �˸�â�� ��Ÿ���� ����
        yield return uiDelay1;              // �˸�â�� ���ӽ�ų �ð�
        subAnimator.SetBool("isOn", false); // �˸�â�� ������� ����
        yield return uiDelay2;              // ���
        subbox.SetActive(false);
    }
}
