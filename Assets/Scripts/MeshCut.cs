using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCut : MonoBehaviour
{
    // [SerializeField] private float limit = 0f;
    [SerializeField] private float fadeDuration = 0.5f; // Duration of the fade effect in seconds
    [SerializeField] private bool DoFade = false;
    [SerializeField] private float fadeAmount = 0.5f;
    [SerializeField] private float limit = 0f;
     private float InitialAlpha;


    private void Start()
    {
        InitialAlpha = transform.GetChild(0).GetComponent<Renderer>().material.color.a;
    }

    // Update is called once per frame
    void Update()
    {

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Material mat = child.GetComponent<Renderer>().material;
                if (child.position.x < limit)
                {
                    StartCoroutine(FadeOut(mat,child.gameObject));
                    
                }
                else
                {
                    StartCoroutine(FadeIn(mat,child.gameObject));
                    
                }
            }
         
            
    }

    IEnumerator FadeIn(Material mat, GameObject child)
    {
        var startAlpha = mat.color.a;
        child.gameObject.SetActive(true);
        for (float f =startAlpha ; f <= InitialAlpha; f += fadeAmount)
        {
            Color c =mat.color;
            c.a = f;
            mat.color = c;
            yield return new WaitForSeconds(fadeDuration);
        }

        // print("do fade");

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
        // print("do fade");
    }


}
