using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeShape : MonoBehaviour
{
    [SerializeField] private float[] distances;
    [SerializeField] private float aprroximate= 0.1f;
    [SerializeField] private GameObject[] positionsGameObjects;
    [SerializeField] private Row[] grid;

    [SerializeField] private GameObject objectToShown;
    // Start is called before the first frame update
    private static bool _recognizeShape = false;
    private bool flag = true;

    // Update is called once per frame
    void Update()
    {
        flag = true;
        int i = 0;
        //for multiple we have grid with row that hold two game objects and the distance between them
        foreach (var row in grid)
        {
            var dist = Vector3.Distance(row.positions[0].transform.position, row.positions[1].transform.position);
            print(dist + " distance");
            if ((row.distance-aprroximate) >=dist || dist>= row.distance+aprroximate)
            {    
                print(" _recognizeShape failed");
                flag = false;            
            } 
        }
        if (flag){
            _recognizeShape = true;
            objectToShown.gameObject.SetActive(true);
            print(" _recognizeShape success");
        }
     
        // var distance = Vector3.Distance(positionsGameObjects[0].transform.position, positionsGameObjects[1].transform.position);
        // if (distances[i]-aprroximate<=distance && distance<= distances[i]+aprroximate)
        // {      
        //     print(-aprroximate+distances[i] + " distance");
        //     _recognizeShape = true;
        //     objectToShown.gameObject.SetActive(true);
        // }
        // else{
        //     print(" _recognizeShape failed");
        //     flag = false;
        // }
    }

    public static bool GetRecognizeShape()
    {
        return _recognizeShape;
    }
}
