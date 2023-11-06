using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowState : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] TextMeshProUGUI clockText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI heartRateText;
    [SerializeField] TextMeshProUGUI stepsText;

    [SerializeField] Slider progressSlider;
    float progressNum = 0;    // progressSlider에 적용할 data 값
    [SerializeField] GameObject player;
    int[] timeSections = new int[] { 5 * 60, 4 * 60, 4 * 60, 4 * 60, 4 * 60, 4 * 60, 5 * 60 };
    float[] startNum = new float[] { 0f, 5f/30f, 9f/30f, 13f/30f, 17f/30f, 21f/30f, 25f/30f };
    

    //[SerializeField] TutorialPlayerController playerController; // gameManager로 대체할 것 
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] TextMeshProUGUI highIntensityText;
    [SerializeField] TextMeshProUGUI moderateIntensityText;

    string[] sectionName = new string[] { "Warm-up", "High-intensity", "Moderate-intensity", "High-intensity", "Moderate-intensity", "High-intensity", "Cool-down" };
    [SerializeField] TextMeshProUGUI sectionText;

    int heartRateRange;

    // Start is called before the first frame update
    void Start()
    {
        ShowThreshold(GameManager.highIntensityHeartRate, GameManager.moderateIntensityHeartRate);
        Debug.Log(GameManager.highIntensityHeartRate + " / " + GameManager.moderateIntensityHeartRate);

        heartRateRange = (GameManager.highIntensityHeartRate - GameManager.moderateIntensityHeartRate);

        // 진행바 % 반영하기 

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        ShowScore();
        ShowProgress();
        ShowHeartRateRange();
        heartRateText.text = gameManager.heartRate.ToString();
        stepsText.text = gameManager.steps.ToString();
    }

    private void FixedUpdate()
    {
        Timer();
    }

    private void Timer()
    {
        GameManager.time[GameManager.indexOfSection] += Time.deltaTime;
        clockText.text = string.Format("{0:D2}:{1:D2}", (int)(GameManager.time[GameManager.indexOfSection] / 60 % 60), (int)(GameManager.time[GameManager.indexOfSection] % 60));
    }

    private void ShowScore()
    {
        scoreText.text = GameManager.score.ToString();
    }

    private void ShowProgress()
    {
        sectionText.text = sectionName[GameManager.indexOfSection];
        progressNum = GameManager.time[GameManager.indexOfSection];
        progressSlider.value = startNum[GameManager.indexOfSection] + (float)(progressNum / timeSections[GameManager.indexOfSection]);
        //Debug.Log("progress : " + startNum[GameManager.indexOfSection] + "/" + progressSlider.value);
    }



    private void ShowHeartRateRange()
    {
        if (gameManager.heartRate > 180)
        {
            scrollbar.value = 0.9f;
        }
        else if (gameManager.heartRate < 60)
        {
            scrollbar.value = 0.1f;
        }
        else
        {
            scrollbar.value = ((gameManager.heartRate - 60f) / (180f - 60f)) * (0.9f - 0.1f) + 0.2f;
        }
    }

    private void ShowThreshold(int highIntensityHeartRate, int moderateIntensityHeartRate)
    {
        highIntensityText.text = highIntensityHeartRate.ToString();
        moderateIntensityText.text = moderateIntensityHeartRate.ToString();
    }
}
