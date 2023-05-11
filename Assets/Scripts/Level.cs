using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Level
{
    private int _levelNumber;
    private int _splitWorldNumber;

    protected Level(int levelNumber, int splitWorldNumber)
    {
        _levelNumber = levelNumber;
        _splitWorldNumber = splitWorldNumber;
    }
}

public class Level1 : Level
{
    public Level1(int levelNumber, int splitWorldNumber) : base(levelNumber, splitWorldNumber) {}
}
