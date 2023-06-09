using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static int _level = 0;
    private static int _numOfHalves = 0;
    private static LevelManager _instance;
    private static GameObject _panelFade;
    private static bool _fadeOutComplete;
    private static bool _loadScene;
    
    private void Awake()
    {
        SetNumOfHalves();
        _instance = this;
        _panelFade = GameObject.FindWithTag("fade");
        _loadScene = false;
        RegisterFadeOutCallback();
    }
    
    public static void Initialize(GameObject panelFade)
    {
        // _level = 0;
        // _panelFade = panelFade;
        
    }

    private void Update()
    {
        SetNumOfHalves();
    }

    public static void NextLevel()
    {
        _level++;
        if (_level != 8)
        {
            if (Levels.unlockedLevel == _level-1)
                Levels.unlockedLevel += 1;
            _panelFade.GetComponent<Animator>().SetBool("fadeOutBool",true);
        }
        else if (!_loadScene)
        {
            _loadScene = true;
            SceneManager.LoadScene("EndScene");
        }
        
    }
    
    
    private static void RegisterFadeOutCallback()
    {
        Animator fadeAnimator = _panelFade.GetComponent<Animator>();

        // Register a callback for the fade-out animation's "OnComplete" event
        fadeAnimator.GetCurrentAnimatorClipInfo(0);
        AnimationClip[] clips = fadeAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "fade-out")
            {
                AnimationEvent animEvent = new AnimationEvent();
                animEvent.functionName = "FadeOutComplete";
                animEvent.time = clip.length;
                clip.AddEvent(animEvent);
                break;
            }
        }
    }

    public static void FadeOutComplete()
    {
        _fadeOutComplete = true;
        // print("fadeOutComplete");
        LoadNextLevel();
    }

    private static void LoadNextLevel()
    {
        // if (_fadeOutComplete)
        // {
        //     SceneManager.LoadScene("Level" + _level);
        // }
        if (_level != 8 && !_loadScene)
        {
            SceneManager.LoadSceneAsync("Level" + _level,LoadSceneMode.Single);
            _loadScene = true;
        }
    }
    
    public static void SetLevel(int newLevel)
    {
        _level = newLevel;
        // _currentLevel = LevelFactory.CreateLevel(_level);
    }

    public static void SetNumOfHalves()
    {
        switch (_level)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                _numOfHalves = 2;
                break;    
            case 4:
            case 5:
                _numOfHalves = 3;
                break;
            case 6:
                _numOfHalves = 4;
                break;
            case 7:
                _numOfHalves = 4;
                break;
        }
    }

    public static int GetLevel()
    {
        return _level;
    }

    public static int GetNumOfHalves()
    {
        return _numOfHalves;
    }
}
