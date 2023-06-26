using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static int _level = 1;
    private static int _numOfHalves = 2;
    private static LevelManager _instance;
    private static GameObject panelPade;
    
    private void Awake()
    {
        SetNumOfHalves();
        _instance = this;
        panelPade = GameObject.FindWithTag("fade");
    }

    private void Update()
    {
        SetNumOfHalves();
    }

    public static void NextLevel()
    {
        _level++;
        if (Levels.unlockedLevel == _level-1)
            Levels.unlockedLevel += 1;
        panelPade.GetComponent<Animator>().SetTrigger("fadeOut");
        _instance.StartCoroutine(WaitForLoadNextLevel());
        // SceneManager.LoadScene("Level5");
        //  _currentLevel = LevelFactory.CreateLevel(_level);
    }

    static IEnumerator WaitForLoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Level" + _level);
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
                _numOfHalves = 2;
                break;
            case 1:
                _numOfHalves = 2;
                break;
            case 2:
                _numOfHalves = 2;
                break;
            case 3:
                _numOfHalves = 2;
                break;    
            case 4:
                _numOfHalves = 3;
                break;  
            case 5:
                _numOfHalves = 3;
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
