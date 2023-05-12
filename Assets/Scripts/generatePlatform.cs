using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePlatform : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlatform;
    [SerializeField] private int angle = 10;
    [SerializeField] private int numberOfPieces = 6;
    void Start()
    {
        //when pivot on (0,0)
        for (int i = 0; i < numberOfPieces - 1; i++)
        {
            GameObject pos = GetComponentsInChildren<Transform>()
                [transform.childCount].gameObject;
            GameObject piece = Instantiate(prefabPlatform, pos.transform.position, 
                pos.transform.rotation * Quaternion.Euler (0f, angle, 0f));
            piece.transform.parent = transform;
        }
    }

}
