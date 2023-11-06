using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(UpandDown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveUp()
    {
        Vector3 targetPosition = new Vector3(31, 30, 8);
        transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, 0.9f);
    }

    void MoveDown()
    {
        Vector3 targetPosition = new Vector3(31, 10, 8);
        transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, 0.05f);
    }

    IEnumerator Move(Vector3 endPos, float duration)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        Vector3 startPos = transform.localPosition;

        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Time.deltaTime);
            
            yield return waitForEndOfFrame;
        }

        transform.localPosition = endPos;
    }

    IEnumerator UpandDown()
    {
        StartCoroutine(Move(new Vector3(31, 30, 8), 5f));
        Debug.Log("up");
        yield return new WaitForSecondsRealtime(2f);
        StartCoroutine(Move(new Vector3(31, 10, 8), 5f));
        Debug.Log("down");
    }

}
