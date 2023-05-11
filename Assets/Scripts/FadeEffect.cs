using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    
    private Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;

    }

    private void Update()
    {
        float time = material.GetFloat("_StartTime");
        time += Time.deltaTime;
        material.SetFloat("_StartTime", time);
    }
}