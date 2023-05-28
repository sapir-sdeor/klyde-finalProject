using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    private void Update()
    {
        if (!RecognizeShape.GetRecognizeShape() || LevelManager.GetLevel() != 1) return;
        hint.SetActive(false);
    }
}