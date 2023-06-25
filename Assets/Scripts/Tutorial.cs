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
    private bool _rotateOnce,_rotateHintShow;

    private void Start()
    {
        if (LevelManager.GetLevel() == 2)
        {
            rotateHint.SetActive(false);
        }
    }


    private void Update()
    {
        if (moving.GetIsWalk())
        {
            walkHint.SetActive(false);
            if (LevelManager.GetLevel() == 2 && !_rotateOnce)
            {
                rotateHint.SetActive(true);
                _rotateHintShow = true;
            }
        }
            
        else if (Rotate2D3D.GetIsRotating() && LevelManager.GetLevel() == 0)
            rotateHint.SetActive(false);

        if (_rotateHintShow && Rotate2D3D.GetIsRotating())
        {
            rotateHint.SetActive(false);
            _rotateOnce = true;
        } 
    }
}
  
