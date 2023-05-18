using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Tutorial : World
{
    [SerializeField] private GameObject walkHint;
    [SerializeField] private GameObject rotateHint;


    private void Update()
    {
        if (moving.GetIsWalk())
            walkHint.SetActive(false);
        else if (Rotate2D3D.GetIsRotating())
            rotateHint.SetActive(false);
    }
}
  
