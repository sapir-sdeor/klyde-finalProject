using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeShape : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;

    [SerializeField] private GameObject[] positionsGameObjects;

    [SerializeField] private float sphereRadius = 0.1f;
    // Start is called before the first frame update
    private static bool _recognizeShape = false;
    private bool flag = true;

    // Update is called once per frame
    void Update()
    {
        flag = true;
        for (int i = 0; i < positionsGameObjects.Length; i++)
        {

            if (!IsInSphere(positionsGameObjects[i].transform.position, positions[i], sphereRadius))
            {
                print(" _recognizeShape failed");
                flag = false;
            }
        }
        if (flag)
        {
            print(" _recognizeShape successfully");
            _recognizeShape = true;

        }
    }
    
    private bool IsInSphere(Vector3 gameObjectPosition, Vector3 sphereCenterPosition, float sphereRadius)
    {
        // Calculate the distance between the game object and the sphere center
        float distance = Vector3.Distance(gameObjectPosition, sphereCenterPosition);
        // print("distance "+ distance);

        // Check if the game object is within the sphere
        if (distance <= sphereRadius)
        {
            return true; // Game object is within the sphere
        }

        return false;
    }

    public static bool GetRecognizeShape()
    {
        return _recognizeShape;
    }
}
