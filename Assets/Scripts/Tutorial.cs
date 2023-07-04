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
    [SerializeField] private float _timeToDisapear = 1;
    private bool _rotateOnce,_rotateHintShow, _disRotate, _disWalk;
    private float _timeRotate, _timeWalk;

    private void Start()
    {
        if (LevelManager.GetLevel() == 2)
        {
            rotateHint.SetActive(false);
        }
    }


    private void Update()
    {
        if (_disWalk)
        {
            _timeWalk += Time.deltaTime;
            if (_timeWalk > _timeToDisapear)
                walkHint.GetComponent<Animator>().SetBool("disappear", true);
        }
        if (_disRotate)
        {
            _timeRotate += Time.deltaTime;
            if (_timeRotate > _timeToDisapear)
                rotateHint.GetComponent<Animator>().SetBool("disappear", true);
        }
        if (moving.GetIsWalk())
        {
            _disWalk = true;
            if (!_rotateOnce)
            {
                rotateHint.SetActive(true);
                _rotateHintShow = true;
            }
        }
        /*else if (Rotate2D3D.GetIsRotatingMore() && LevelManager.GetLevel() == 0)
            disRotate = true;*/
        if (_rotateHintShow && Rotate2D3D.GetIsRotatingMore())
        {
            _rotateOnce = true;
            _disRotate = true;
        } 
    }
    
}
  
