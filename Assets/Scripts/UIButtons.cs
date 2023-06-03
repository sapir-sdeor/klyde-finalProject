using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public static bool isPause;
    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        isPause = true;
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }

    public void Menu()
    {
        //TODO: set to menu scene
        SceneManager.LoadScene("Levels");
    }
}