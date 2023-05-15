using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static int _level = 1;
    private Level _currentLevel;
    private static int _numOfHalfs = 2;

    private void Update()
    {
        SetNumOfHalfs();
    }

    public void NextLevel()
    {
        _level++;
        _currentLevel = LevelFactory.CreateLevel(_level);
    }
    
    public void SetLevel(int newLevel)
    {
        _level = newLevel;
        _currentLevel = LevelFactory.CreateLevel(_level);
    }
    
    private void SetNumOfHalfs()
    {
        switch (_level)
        {
            case 1:
            case 2:
                _numOfHalfs = 2;
                break;
            case 3:
                _numOfHalfs = 3;
                break;           
        }
    }

    public static int GetLevel()
    {
        return _level;
    }

    public static int GetNumOfHalfs()
    {
        return _numOfHalfs;
    }
}
