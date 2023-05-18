using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] worlds;
    [SerializeField] private Texture[] textureWorlds;
    [SerializeField] private GameObject hint;
    private void Awake()
    {
        _levelManager = GetComponent<LevelManager>();
    }

    public void RecognizeShape()
    {
        if (_levelManager.GetLevel() > 1)
            door.SetActive(true);
        if (_levelManager.GetLevel() == 2) //tutorial2 - needs to remove hint
            hint.SetActive(false);
        for (int i=0; i<worlds.Length; i++)
        {
            worlds[i].GetComponent<MeshRenderer>().material.mainTexture = textureWorlds[i];
        }
    }
}