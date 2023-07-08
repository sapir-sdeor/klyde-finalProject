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
    [SerializeField] private Sprite mute;
    [SerializeField] private Sprite unMute;
    public static bool isPause;
    public static bool isMute;
    

    private void Start()
    {
        isPause = false;
        Time.timeScale = 1;
        if (isMute && panel)
        {
            panel.GetComponentsInChildren<Image>()[1].sprite = mute;
        }
        else if (panel)
        {
            panel.GetComponentsInChildren<Image>()[1].sprite = unMute;
        }
    }

    public void StartGame()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("gameManager");
        if(obj)
            Destroy(obj);
        isPause = false;
        Time.timeScale = 1;
        Levels.unlockedLevel = 0;
        LevelManager.SetLevel(0);
        LevelManager.SetNumOfHalves();
        SceneManager.LoadScene("Start");
    }

    public void BackToStart()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("gameManager");
        if(obj)
            Destroy(obj);
        isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Start");
    }

    public void Mute()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("gameManager");
        isMute = !isMute;
        if (isMute)
        {
            obj.GetComponent<AudioSource>().Stop();
            panel.GetComponentsInChildren<Image>()[1].sprite = mute;
        }
        else
        {
            obj.GetComponent<AudioSource>().Play();
            panel.GetComponentsInChildren<Image>()[1].sprite = unMute;
        }
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