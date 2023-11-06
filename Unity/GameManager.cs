using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    [SerializeField] public int age = 0;

    public int heartRate = 0;
    static public int highIntensityHeartRate = 0;
    static public int moderateIntensityHeartRate = 0;
    public int steps = 1;

    public int average = 0;
    static public int score = 0;

    static public float[] time = new float[] { 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] static public int indexOfSection = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로 생기는 인스턴스를 삭제
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(GetHeartRate());
        StartCoroutine(SetScore());
    }

    // 운동강도의 기준 정하기 
    public void SetIntensity()
    {
        if(age >= 17 && age <= 70)
        {
            int maximumHeartRate = 220 - age;
            highIntensityHeartRate = (int)(maximumHeartRate * 0.75);
            moderateIntensityHeartRate = (int)(maximumHeartRate * 0.60);
            Debug.Log("고강도 심박수 기준" + highIntensityHeartRate + " / 중강도 심박수 기준 : " + moderateIntensityHeartRate);
        }
    }

    //async public void GetAverageHeartRate5()
    //{
    //    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //    Query testCollectionRef = db.Collection("apple-health").OrderByDescending("heartDate").Limit(5);
    //    QuerySnapshot snapshot = await testCollectionRef.GetSnapshotAsync();

    //    int sum = 0;

    //    foreach (DocumentSnapshot document in snapshot.Documents)
    //    {
    //        Dictionary<string, object> documentDictionary = document.ToDictionary();

    //        int number = 0;
    //        Int32.TryParse(documentDictionary["heartRate"] as string, out number);
    //        sum += number;
    //    }

    //    sum /= 5;
    //    average = sum;
    //    Debug.Log("average Heart Rate : " + average);
    //}

    // steps와 heartrate 받아오기
    async public void GetHeartRateDatebase()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query testCollectionRef = db.Collection("apple-health").OrderByDescending("heartDate").Limit(1);
        QuerySnapshot snapshot = await testCollectionRef.GetSnapshotAsync();

        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();

            int number = 0;
            Int32.TryParse(documentDictionary["heartRate"] as string, out number);
            heartRate = number;

            int number_steps = 0;
            Int32.TryParse(documentDictionary["steps"] as string, out number_steps);
            steps = number_steps;
        }

        //Debug.Log("Heart Rate : " + heartRate + " / steps : " + steps);
    }

    IEnumerator GetHeartRate()
    {
        while (true)
        {
            GetHeartRateDatebase();
            if (steps > 0)
            {
                score += 10;
            }

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    IEnumerator SetScore()
    {
        if (steps > 0)
        {
            score += 10;
            yield return new WaitForSecondsRealtime(1f);
            yield return StartCoroutine(SetScore());
        }
    }

    public bool CheckRunningState()
    {
        if (steps <= 0)
            return false;
        else
            return true;
    }

    public void IncreaseIndexOfSection()
    {
        indexOfSection++;
    }
}
