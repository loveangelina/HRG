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
        // Gamemanager에 heartRate를 만들어서 가져올지, 플레이어에서 받아서 플레이어꺼를 가져올지 
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
