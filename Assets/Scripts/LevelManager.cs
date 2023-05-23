using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int _level = 2;
    // [SerializeField] private int Level = 3;
    private Level _currentLevel;
    private static int _numOfHalves = 2;

    // private void Start()
    // {
    //     _level = Level;
    // }

    private void Awake()
    {
        SetNumOfHalves();
    }

    private void Update()
    {
        SetNumOfHalves();
    }

    public static void NextLevel()
    {
        _level++;
        SceneManager.LoadScene("Level" + _level);
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
                _numOfHalves = 2;
                break;
            case 2:
                _numOfHalves = 2;
                break;
            case 3:
                _numOfHalves = 3;
                break;           
        }
    }

    public int GetLevel()
    {
        return _level;
    }

    public static int GetNumOfHalves()
    {
        return _numOfHalves;
    }
}
