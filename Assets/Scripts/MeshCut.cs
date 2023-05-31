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
    [SerializeField] private float offset = 0;


    private void Start()
    {
        // _initialAlpha = transform.GetChild(0).GetComponent<Renderer>().material.color.a;
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }

    // Update is called once per frame
    void Update()
    { 
          CallFade(); 
    }
    
    // private void CallFade()
    // {
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         Transform child = transform.GetChild(i);
    //         var currAngle = child.eulerAngles.y;
    //         var parentAngle = transform.parent.eulerAngles.z;
    //         currAngle += parentAngle;
    //         if ((_angle)*(halfNum-1) -angleFadeEffect <= currAngle && currAngle <= (_angle)*(halfNum)+angleFadeEffect)
    //         {
    //             child.gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             child.gameObject.SetActive(false);      
    //         }
    //     }
    // }
    private void CallFade()
    {
        int numOfHalves = LevelManager.GetNumOfHalves();
        float angleRange = 360f / numOfHalves;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            var currAngle = child.eulerAngles.y;
            var parentAngle = transform.parent.eulerAngles.z;
            currAngle += parentAngle;

            // Calculate the adjusted angle range based on the number of halves
            float startAngle = offset + angleRange * (halfNum - 1);
            float endAngle = offset + angleRange * halfNum;

            // Handle wraparound from 360 to 0 degrees
            if (endAngle > 360f)
                endAngle -= 360f;

            // Check if the current angle is within the adjusted range
            if (IsAngleWithinRange(currAngle, startAngle, endAngle, angleFadeEffect))
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

// Helper method to check if an angle is within a specified range
    private bool IsAngleWithinRange(float angle, float start, float end, float tolerance)
    {
        // Normalize the angles to ensure proper comparison
        angle = NormalizeAngle(angle);
        start = NormalizeAngle(start);
        end = NormalizeAngle(end);

        // Check if the angle is within the specified range with tolerance
        if (start <= end)
        {
            return angle >= start - tolerance && angle <= end + tolerance;
        }
        else
        {
            return angle >= start - tolerance || angle <= end + tolerance;
        }
    }

// Helper method to normalize an angle to the range of 0-360 degrees
    private float NormalizeAngle(float angle)
    {
        if (angle < 0f)
        {
            angle %= 360f;
            angle += 360f;
        }
        else if (angle >= 360f)
        {
            angle %= 360f;
        }
        return angle;
    }
}
