using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeShape : MonoBehaviour
{
    [SerializeField] private float[] distances;
    [SerializeField] private float aprroximate= 0.1f;
    [SerializeField] private GameObject[] positionsGameObjects;

    [SerializeField] private GameObject objectToShown;
    // Start is called before the first frame update
    private static bool _recognizeShape = false;
    private bool flag = true;

    // Update is called once per frame
    void Update()
    {
        flag = true;
        int i = 0;
        // foreach (var pointsAB in positionsGameObjects)
        // {
        // }
        // i++;
        var distance = Vector3.Distance(positionsGameObjects[0].transform.position, positionsGameObjects[1].transform.position);
        if (distances[i]-aprroximate<=distance && distance<= distances[i]+aprroximate)
        {      
            print(-aprroximate+distances[i] + " distance");
            _recognizeShape = true;
            objectToShown.gameObject.SetActive(true);
        }
        else{
            print(" _recognizeShape failed");
            flag = false;
        }
    }
    
    // private bool IsInSphere(Vector3 gameObjectPosition, Vector3 sphereCenterPosition, float sphereRadius)
    // {
    //     // Calculate the distance between the game object and the sphere center
    //     float distance = Vector3.Distance(gameObjectPosition, sphereCenterPosition);
    //     print("distance "+ distance);
    //
    //     // Check if the game object is within the sphere
    //     if (distance <= sphereRadius)
    //     {
    //         return true; // Game object is within the sphere
    //     }
    //
    //     return false;
    // }

    public static bool GetRecognizeShape()
    {
        return _recognizeShape;
    }
}
