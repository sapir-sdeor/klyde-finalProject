using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePlatform : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlatform;
    [SerializeField] private int angle = 1;
    [SerializeField] private int numberOfPieces = 6;
    [SerializeField] private float offsetFactor = 1;
    // [SerializeField] private Vector2 tile = new Vector2(50,1);
    void Awake()
    {
        var propertyBlock = new MaterialPropertyBlock();
        for (int i = 0; i < numberOfPieces - 1; i++)
        {
            //Instantiate
            GameObject pos = GetComponentsInChildren<Transform>()[transform.childCount].gameObject;
            GameObject piece = Instantiate(prefabPlatform, pos.transform.position, pos.transform.rotation * Quaternion.Euler(0f, angle, 0f));
            piece.transform.parent = transform;

            //set offset
            Renderer renderer = piece.GetComponent<Renderer>();
            renderer.GetPropertyBlock(propertyBlock);
            //try 1
            // renderer.material.SetTextureOffset("_MainTex", new Vector2(piece.transform.eulerAngles.y, 0));
            //try 2
            print("i :" + i);
            float j = i * (1f / numberOfPieces);
            print(j);
            renderer.material.SetTextureOffset("_MainTex", new Vector2(0, j));
            //try 3
            // renderer.material.SetTextureOffset("_MainTex", new Vector2(i*offsetFactor, i*offsetFactor));
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}



