using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerImage : MonoBehaviour
{
    [SerializeField] GameObject dangerImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gamemanager�� heartRate�� ���� ��������, �÷��̾�� �޾Ƽ� �÷��̾�� �������� 
        //if (heartRate < 100)
        //    StartCoroutine(ToggleUIImage());
    }

    IEnumerator ToggleUIImage()
    {
        dangerImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        dangerImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

    }
}
