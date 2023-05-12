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
                    FadeOut(mat,child.gameObject);
                    
                }
                else
                {
                    FadeIn(mat,child.gameObject);
                }
            }
         
            
    } 

    void FadeIn(Material mat, GameObject child)
        =>  StartCoroutine(Fade(mat, child, InitialAlpha, true));
    void FadeOut(Material mat, GameObject child)
        =>  StartCoroutine(Fade(mat, child, 0, false));

    IEnumerator Fade(Material mat, GameObject child, float target, bool activeStart)
    {
        var startAlpha = mat.color.a;
        
        child.gameObject.SetActive(activeStart);
        var startT = Time.time;
        Color c =mat.color;
            .,l
        while (Time.time < startT + fadeDuration)
        {
            var fraction = (Time.time - startT) / fadeDuration;
            var f = Mathf.Lerp(startAlpha, target, 1-fraction);
            c.a = f;
            mat.color = c;
            c = mat.color;
            yield return new WaitForEndOfFrame();
        }
        c.a = target;
        mat.color = c;
        child.gameObject.SetActive(!activeStart);
        print("do fade");
    }
   


}
