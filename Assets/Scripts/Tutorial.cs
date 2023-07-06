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
    private bool _rotateOnce,_rotateHintShow, _disRotate, _disWalk, _disHeart,_disButton, _disappearHint;
    private float _timeRotate, _timeWalk, _timeAppear, _timeHeart,_timeButton;
    private GameObject _klea;
    private void Start()
    {
        _klea = GameObject.FindWithTag("klyde");
        if (LevelManager.GetLevel() == 2)
        {
            rotateHint.SetActive(false);
        }
    }


    private void Update()
    {
        switch (LevelManager.GetLevel())
        {
            case 0:
                Level0Hint();
                break;
            case 1:
                Level1Hint();
                break;
            case 2:
                Level2Hint();
                break;
        }
    }

    private void Level0Hint()
    {
        if (_disButton)
        {
            if (Door.GetWin())
                buttonHint.SetActive(false);
        }
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
        if (moving.GetIsWalk())
        {
            _disWalk = true;
            if (!_rotateOnce)
            {
                _rotateHintShow = true;
            }
        }
       
        if (_rotateHintShow && Rotate2D3D.GetIsRotatingMore())
        {
            _rotateOnce = true;
            _disRotate = true;
        }
        
        if (_rotateHintShow && !rotateHint.activeSelf)
        {
            _timeAppear += Time.deltaTime;
            if (_timeAppear > timeToAppear)
            {
                rotateHint.SetActive(true);
            }
        }

    }
    
    private void Level1Hint()
    {
        if (_disHeart)
        {
            _timeHeart += Time.deltaTime;
            if (_timeHeart > timeToDisapear)
                completeTheHeartHint.SetActive(false);
        }
        if (RecognizeShape.GetRecognizeShape())
        {
            _disHeart = true;
        }
    }
    
    private void Level2Hint()
    {
        if (_disWalk)
        {
            _timeWalk += Time.deltaTime;
            if (_timeWalk > timeToDisapear)
                walkHint.SetActive(false);
        }
        if (_disRotate)
        {
            print("dis rotate");
            _timeRotate += Time.deltaTime;
            print("time " + _timeRotate);
            if (_timeRotate > timeToDisapear)
            {
                rotateHint.SetActive(false);
                _disappearHint = true;
            }
                
        }

        if (_klea.transform.position.x > 0)
        {
            _disWalk = true;
            if (!_rotateOnce && !_rotateHintShow)
            {
                print("rotate hint show");
                _rotateHintShow = true;
            }
        }
        
        if (_rotateHintShow && Rotate2D3D.GetIsRotatingMore())
        {
            print("start disappear");
            _rotateOnce = true;
            _disRotate = true;
        }
        
        if (_rotateHintShow && !_disappearHint)
        {
            _timeAppear += Time.deltaTime;
            if (_timeAppear > timeToAppear)
            {
                print("set active");
                rotateHint.SetActive(true);
            }
        }
    }
    
}
  
