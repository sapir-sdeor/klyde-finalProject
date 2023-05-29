using System;
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

    [SerializeField] private GameObject background;
    [SerializeField] private Texture backgroundAfterShape;
    // Start is called before the first frame update
    private static bool _recognizeShape = false;
    private float _angle;
    private bool flag = true;
    private float _timeToDisappear;

    private void Start()
    {
        _recognizeShape = false;
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }

    // Update is called once per frame
    void Update()
    {
        flag = true;
        int i = 0;
        if (_recognizeShape)
            _timeToDisappear += Time.deltaTime;
        if (_timeToDisappear > 3)
            background.GetComponent<MeshRenderer>().material.mainTexture = backgroundAfterShape;

        if (_timeToDisappear > 5)
            objectToShown.gameObject.SetActive(false);
        
            
        //for multiple we have grid with row that hold two game objects and the distance between them
        foreach (var row in grid)
        {
            var dist = Vector3.Distance(row.positions[0].transform.position, row.positions[1].transform.position);
            // print(dist + " distance");
            if ((row.distance-aprroximate >=dist || dist>= row.distance+aprroximate || PointsInRightHalf(row)))
            {    
                // print(" _recognizeShape failed");
                flag = false;            
            } 
        }
        if (flag && !_recognizeShape){
            _recognizeShape = true;
            objectToShown.gameObject.SetActive(true);
        }
        
    }

    public static bool GetRecognizeShape()
    {
        return _recognizeShape;
    }

    private bool PointsInRightHalf(Row row)
    {
        for(int i=0; i < 2 ;i++)
        {
            Transform trans =row.positions[i].transform;
            Vector3 direction = trans.position - Vector3.zero;
            // Calculate the angle between the direction vector and the forward vector
            float currAngle = Vector3.Angle(Vector3.forward, direction);
            if (trans.position.x < 0) currAngle = 360 - currAngle;
            if (!(_angle * (row.halfNumPoints[i] - 1) <= currAngle && currAngle <= _angle * (row.halfNumPoints[i])))
            {
                return false;
            }
        }
        return true;
    }
    
    
}
