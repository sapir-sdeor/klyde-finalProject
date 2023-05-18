using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCut : MonoBehaviour
{
    // [SerializeField] private float limit = 0f;
    // [SerializeField] private float fadeDuration = 0.001f; // Duration of the fade effect in
    // [SerializeField] private float fadeAmount = 0.5f;
    // private float _initialAlpha;
    [SerializeField] private int halfNum;
    private float _angle;
    [SerializeField] private float angleFadeEffect = 5;


    private void Start()
    {
        // _initialAlpha = transform.GetChild(0).GetComponent<Renderer>().material.color.a;
        _angle = 360 /(float) LevelManager.GetNumOfHalfs();
    }

    // Update is called once per frame
    void Update()
    { 
          CallFade(); 
    }
    
    private void CallFade()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            var currAngle = child.eulerAngles.y;
            var parentAngle = transform.parent.eulerAngles.z;
            currAngle += parentAngle;
            // if(i==1) Debug.Log("Object " + i + " angle: " + currAngle);
            // if(i== transform.childCount-1) Debug.Log("Object " + i + " angle: " + currAngle);
            if ( _angle*(halfNum-1) -angleFadeEffect <= currAngle && currAngle <= _angle*(halfNum)+angleFadeEffect)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);      
            }
        }
    }
    
    
}
