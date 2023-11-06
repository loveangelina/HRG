using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Distance : MonoBehaviour
{
    [SerializeField] GameObject dragon;
    [SerializeField] GameObject player;
    [SerializeField] LineRenderer line;
    [SerializeField] TextMeshPro distanceText;

    void Start()
    {
        line.positionCount = 2;
        
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, dragon.transform.position);
        distanceText.text = ((int)distance).ToString();
        distanceText.transform.position = new Vector3((2*line.GetPosition(0).x + 3*line.GetPosition(1).x) / 5, 0.9f, 0);

        line.SetPosition(0, dragon.transform.position + new Vector3(-2, 0.2f, 0));
        line.SetPosition(1, player.transform.position + new Vector3(0, 0.2f, 0));
    }
}
