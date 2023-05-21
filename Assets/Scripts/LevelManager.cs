using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int _level = 1;
    private Level _currentLevel;
    private static int _numOfHalfs = 2;
    
    private void Update()
    {
        SetNumOfHalves();
    }

    public static void NextLevel()
    {
        _level++;
        SceneManager.LoadScene("Level " + _level);
        //  _currentLevel = LevelFactory.CreateLevel(_level);
    }
    
    public void SetLevel(int newLevel)
    {
        _level = newLevel;
        _currentLevel = LevelFactory.CreateLevel(_level);
    }
    
    private void SetNumOfHalves()
    {
        switch (_level)
        {
            case 1:
                _numOfHalfs = 2;
                break;
            case 2:
                _numOfHalfs = 2;
                break;
            case 3:
                _numOfHalfs = 3;
                break;           
        }
    }

    public int GetLevel()
    {
        return _level;
    }

    public static int GetNumOfHalves()
    {
        return _numOfHalfs;
    }
}
