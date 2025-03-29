using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : MonoBehaviour
{
    private float timer = 0f;
    private float interval = 0.1f;
    private float totalElapsedTime = 0f;
    private float duration = 4.033f;
    [HideInInspector] public List<float> xPos;
    private bool done = false;

    void Start()
    {
        xPos.Add(gameObject.transform.position.x);
    }

    
    void Update()
    {
        if (totalElapsedTime < duration)
        {
            timer += Time.deltaTime;
            totalElapsedTime += Time.deltaTime;

            if (timer > interval)
            {
                xPos.Add(gameObject.transform.position.x);
                float speed = Mathf.Abs(xPos[xPos.Count - 1] - xPos[xPos.Count - 2])/interval;
                Debug.Log("id: " + (xPos.Count - 2) + " | " + speed + " m/s");
                timer = 0f;
            }
        }
        if (totalElapsedTime >= duration && !done)
        {
            Debug.Log("list count: " + xPos.Count + ", interval: " + 1f/xPos.Count);
            done = true;
        }
    }
}
