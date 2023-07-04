using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeReciver : MonoBehaviour
{
    public void FadeOutComplete()
    {
        LevelManager.FadeOutComplete();
        print("fade out complete");
    }
}
