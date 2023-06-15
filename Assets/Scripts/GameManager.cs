using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(audioSource);
    }
    
}