using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static int _level;
    private Level _currentLevel;

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
}
