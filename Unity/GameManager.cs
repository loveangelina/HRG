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
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ�
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
        // �ν��Ͻ��� �����ϴ� ��� ���� ����� �ν��Ͻ��� ����
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

    // ������� ���� ���ϱ� 
    public void SetIntensity()
    {
        if(age >= 17 && age <= 70)
        {
            int maximumHeartRate = 220 - age;
            highIntensityHeartRate = (int)(maximumHeartRate * 0.75);
            moderateIntensityHeartRate = (int)(maximumHeartRate * 0.60);
            Debug.Log("���� �ɹڼ� ����" + highIntensityHeartRate + " / �߰��� �ɹڼ� ���� : " + moderateIntensityHeartRate);
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

    // steps�� heartrate �޾ƿ���
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
