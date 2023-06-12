using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static int _level = 3;
    private static int _numOfHalves = 2;
    private static LevelManager _instance;
    
    private void Awake()
    {
        SetNumOfHalves();
        _instance = this;
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
        _instance.StartCoroutine(WaitForLoadNextLevel());
        //SceneManager.LoadScene("Level" + _level);
        //  _currentLevel = LevelFactory.CreateLevel(_level);
    }

    static IEnumerator WaitForLoadNextLevel()
    {
        yield return new WaitForSeconds(0.01f);
        SceneManager.LoadScene("Level" + _level);
    }
    
    public static void SetLevel(int newLevel)
    {
        _level = newLevel;
       // _currentLevel = LevelFactory.CreateLevel(_level);
    }
    
    private void SetNumOfHalves()
    {
        switch (_level)
        {
            case 1:
                _numOfHalves = 2;
                break;
            case 2:
                _numOfHalves = 2;
                break;
            case 3:
                _numOfHalves = 3;
                break;    
            case 4:
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
