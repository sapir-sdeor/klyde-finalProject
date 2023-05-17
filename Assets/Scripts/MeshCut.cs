using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCut : MonoBehaviour
{
    // [SerializeField] private float limit = 0f;
    [SerializeField] private float fadeDuration = 0.01f; // Duration of the fade effect in
    [SerializeField] private float fadeAmount = 0.5f;
    private float _initialAlpha;
    [SerializeField] private int halfNum;
    private float _angle;


    private void Start()
    {
        _initialAlpha = transform.GetChild(0).GetComponent<Renderer>().material.color.a;
        _angle = 360 /(float) LevelManager.GetNumOfHalfs();
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
    //
    //         // Calculate the direction vector from (0, 0, 0) to the object
    //         Vector3 absolutePosition = child.TransformPoint(Vector3.zero);
    //         Vector3 direction = absolutePosition - Vector3.zero;
    //
    //         // Calculate the angle between the direction vector and the forward vector using Euler angles
    //         Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.forward);
    //         float currAngle = targetRotation.eulerAngles.z;
    //
    //         if (child.position.x < 0) currAngle = 360f - currAngle;
    //
    //         if (i == 1)
    //         {
    //             print(currAngle + " 1");
    //         }
    //         if (i == transform.childCount - 1)
    //         {
    //             print(currAngle + " last");
    //         }
    //
    //         Material mat = child.GetComponent<Renderer>().material;
    //         if (_angle * (halfNum - 1) <= currAngle && currAngle <= _angle * halfNum)
    //         {
    //             StartCoroutine(FadeIn(mat, child.gameObject));
    //         }
    //         else
    //         {
    //             StartCoroutine(FadeOut(mat, child.gameObject));
    //         }
    //     }
    // }


    private void CallFade()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            // Calculate the direction vector from (0, 0, 0) to the object
            Vector3 absolutePosition = child.TransformPoint(Vector3.zero);
            Vector3 direction = absolutePosition - Vector3.zero;
            // Calculate the angle between the direction vector and the forward vector
            float currAngle = Vector3.Angle(Vector3.forward, direction);
            if (child.position.x < 0) currAngle = 360 - currAngle;
            if (i == 1)
            {
                // print(currAngle+" 1");
            }
            if (i == transform.childCount-1)
            {
                // print(currAngle+" last");
            }
            
            Material mat = child.GetComponent<Renderer>().material;
            if ( _angle*(halfNum-1) <= currAngle && currAngle <= _angle*(halfNum))
            {
                StartCoroutine(FadeIn(mat,child.gameObject));       
            }
            else
            {
                StartCoroutine(FadeOut(mat,child.gameObject));         
            }
        }
    }

    IEnumerator FadeIn(Material mat, GameObject child)
    {
        var startAlpha = mat.color.a;
        child.gameObject.SetActive(true);
        for (float f =startAlpha ; f <= _initialAlpha; f += fadeAmount)
        {
            Color c =mat.color;
            c.a = f;
            mat.color = c;
            yield return new WaitForSeconds(fadeDuration);
        }
    }
    IEnumerator FadeOut(Material mat, GameObject child)
    {
        var startAlpha = mat.color.a;
        for (float f =startAlpha  ; f >= 0; f -= fadeAmount)
        {
            Color c =mat.color;
            c.a = f;
            mat.color = c;
            yield return new WaitForSeconds(fadeDuration);
        }
        child.gameObject.SetActive(false);;
    }
    
}
