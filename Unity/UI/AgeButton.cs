using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AgeButton : MonoBehaviour
{
    public GameManager gameManager;
    public InputField playerAgeInput;
    [SerializeField] int playerAge = 0;

    public NoticeUI noticeUI;

    public void OnClick()
    {
        try
        {
            playerAge = int.Parse(playerAgeInput.GetComponent<InputField>().text.Trim());
            if (playerAge < 17 || playerAge > 70)
            {
                noticeUI.ShowNotice("17~70 is possible! Try again!");
                playerAge = 0;
            }
            else
            {
                gameManager.age = playerAge;
                gameManager.SetIntensity();
                SceneManager.LoadScene ("Tutorial");
                //SceneManager.LoadScene("1HighIntensityRun");
            }

        }
        catch(FormatException e)
        {
            noticeUI.ShowNotice("Only number is available! Try again!");
            
        }
        
    }
}
