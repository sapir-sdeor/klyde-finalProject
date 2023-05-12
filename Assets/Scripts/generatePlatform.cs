using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePlatform : MonoBehaviour
{
    [SerializeField] private int angle = 10;
    [SerializeField] private int numberOfPieces = 6;
    void Start()
    {
        GameObject toInstantiate = GetComponentInChildren<Transform>().gameObject;
        for (int i = 0; i < numberOfPieces - 1; i++)
        {
            Quaternion newRotation = toInstantiate.transform.rotation;
            newRotation.y += 10;
            GameObject piece = Instantiate(toInstantiate, toInstantiate.transform.position,
                toInstantiate.transform.rotation);
            toInstantiate = piece;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
