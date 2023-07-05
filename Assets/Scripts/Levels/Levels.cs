using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    [SerializeField] private int totalLevel = 7;
    public static int unlockedLevel = 0;
    private ButtonLevel[] levelButtons;
    [SerializeField] private GameObject glow;
    private void Start()
    {
        Refresh();
    }
    
    private void OnEnable()
    {
        levelButtons = GetComponentsInChildren<ButtonLevel>();
    }

    private void Refresh()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            var level = i;
            if (level <= totalLevel)
            {
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].SetUp(level, level<=unlockedLevel);
                print(level<=unlockedLevel);
            }
            else
                levelButtons[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        glow.transform.position = levelButtons[unlockedLevel].transform.position;
    }
}
