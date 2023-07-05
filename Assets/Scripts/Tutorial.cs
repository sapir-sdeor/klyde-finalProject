using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class Tutorial : World
{
    [SerializeField] private GameObject walkHint;
    [SerializeField] private GameObject rotateHint;
    [SerializeField] private GameObject completeTheHeartHint;
    [SerializeField] private float timeToDisapear = 1;
    [SerializeField] private float timeToAppear = 0.5f;
    [SerializeField] private GameObject buttonHint;
    private bool _rotateOnce,_rotateHintShow, _disRotate, _disWalk, _disHeart,_disButton;
    private float _timeRotate, _timeWalk, _timeAppear, _timeHeart,_timeButton;

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
            if (_timeWalk > timeToDisapear)
                walkHint.GetComponent<Animator>().SetBool("disappear", true);
        }
        if (_disRotate)
        {
            _timeRotate += Time.deltaTime;
            if (_timeRotate > timeToDisapear)
            {
                rotateHint.GetComponent<Animator>().SetBool("disappear", true);
                _disButton = true;
                buttonHint.SetActive(true);
            }
      
        }
        if (_disButton)
        {
            if (Door.GetWin())
                buttonHint.SetActive(false);
        }

        if (_disHeart)
        {
            _timeHeart += Time.deltaTime;
            if (_timeHeart > timeToDisapear)
                completeTheHeartHint.SetActive(false);
        }
        
        if (_rotateHintShow && !rotateHint.activeSelf && LevelManager.GetLevel() != 1)
        {
            _timeAppear += Time.deltaTime;
            if (_timeAppear > timeToAppear)
            {
                rotateHint.SetActive(true);
            }
        }
        if (moving.GetIsWalk() && LevelManager.GetLevel() != 1)
        {
            _disWalk = true;
            if (!_rotateOnce)
            {
                _rotateHintShow = true;
            }
        }
        /*else if (Rotate2D3D.GetIsRotatingMore() && LevelManager.GetLevel() == 0)
            disRotate = true;*/
        if (_rotateHintShow && Rotate2D3D.GetIsRotatingMore() && LevelManager.GetLevel() != 1)
        {
            _rotateOnce = true;
            _disRotate = true;
        }

        if (LevelManager.GetLevel() == 1 && RecognizeShape.GetRecognizeShape())
        {
            _disHeart = true;
        }
    }
    
}
  
