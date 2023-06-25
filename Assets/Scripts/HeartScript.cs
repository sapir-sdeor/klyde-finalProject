using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    [SerializeField] private float timeToDisapper = 0.5f;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        print("timeToDisappear" + (Time.time - startTime));
        if(Time.time - startTime > timeToDisapper) gameObject.SetActive(false);
    }
}
