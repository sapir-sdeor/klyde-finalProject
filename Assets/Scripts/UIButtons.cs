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

    private void Start()
    {
        isPause = false;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("gameManager");
        if(obj)
            Destroy(obj);
        isPause = false;
        Time.timeScale = 1;
        LevelManager.SetLevel(0);
        LevelManager.SetNumOfHalves();
        SceneManager.LoadScene("Start");
    }

    public void Menu()
    {
        isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Levels");
    }

    public void CutScene()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        if (Door._win) return;
        panel.SetActive(true);
        Time.timeScale = 0;
        Rotate2D3D.SetStopRotate(false);
        moving.SetStopWalk(false);
        isPause = true;
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }
    
}