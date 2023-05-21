using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class GameManager : MonoBehaviour
{
    private LevelManager _levelManager;
    //[SerializeField] private GameObject door;
    [SerializeField] private GameObject hint;
    private void Awake()
    {
        _levelManager = GetComponent<LevelManager>();
    }

  
    private void Update()
    {
        if (!RecognizeShape.GetRecognizeShape() || _levelManager.GetLevel() != 2) return;
        hint.SetActive(false);
    }
}