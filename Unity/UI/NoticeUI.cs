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
        subAnimator.SetBool("isOn", true);  // 알림창이 나타나는 연출
        yield return uiDelay1;              // 알림창을 지속시킬 시간
        subAnimator.SetBool("isOn", false); // 알림창이 사라지는 연출
        yield return uiDelay2;              // 대기
        subbox.SetActive(false);
    }
}
