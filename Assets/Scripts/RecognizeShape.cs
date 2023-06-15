using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeShape : MonoBehaviour
{
    // [SerializeField] private GameObject[] positionsGameObjects;
    [SerializeField] private Row[] grid;
    [SerializeField] private GameObject objectToShown;

    [SerializeField] private GameObject background;
    [SerializeField] private Texture backgroundAfterShape;
    [SerializeField] private GameObject rightPlane, leftPlane;
    [SerializeField] private GameObject reflect;
    [SerializeField] private float timeToDisappearLimit = 7;
    [SerializeField] private float offset=0;
    // Start is called before the first frame update
    private static bool _recognizeShape ;
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
        // if (_timeToDisappear > 3)
        //     background.GetComponent<MeshRenderer>().material.mainTexture = backgroundAfterShape;
        if (_timeToDisappear > timeToDisappearLimit)
        {
            rightPlane.GetComponent<Animator>().SetBool("recognizeShape",true); 
            leftPlane.GetComponent<Animator>().SetBool("recognizeShape",true);
            objectToShown.gameObject.SetActive(false);
        }

        //for multiple we have grid with row that hold two game objects and the distance between them
        foreach (var row in grid)
        {
            var dist = Vector3.Distance(row.positions[0].transform.position, row.positions[1].transform.position);
            print(dist + " distance " + row.aprroximate +" aprroximate "+ " is rotating? "+ Rotate2D3D.GetIsRotating()+
                " points in right half? "+ !PointsInRightHalf(row) + " name: " +  row.gameObject.name);

            if (LevelManager.GetLevel() == 4 )
            {
                foreach (var pos in row.positions)
                {
                    if (!pos.GetComponent<ReflectPoint>().IsPointSeen())
                    {
                        print(" _recognizeShape failed " + row.name);
                        flag = false; 
                    }
                }
            }
            if (row.distance-row.aprroximate >=dist || dist>= row.distance+row.aprroximate ||Rotate2D3D.GetIsRotating()
                || !PointsInRightHalf(row) && LevelManager.GetLevel() != 4)
            {
                flag = false;
            } 
        }
        if (flag && !_recognizeShape){
            _recognizeShape = true;
            objectToShown.gameObject.SetActive(true);
            if (reflect)
                reflect.gameObject.SetActive(false);
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
            // float currAngle = Vector3.Angle(Vector3.forward, direction);
            // if (trans.position.x < 0) currAngle = 360 - currAngle;
            float currAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            if (currAngle < 0) currAngle += 360f;

            // Calculate the adjusted angle range based on the number of halves
            float startAngle = _angle * (row.halfNumPoints[i] - 1)+offset;
            float endAngle = _angle * row.halfNumPoints[i]+offset;

            // Handle wraparound from 360 to 0 degrees
            if (endAngle > 360f)
                endAngle -= 360f;

            // Check if the current angle is within the adjusted range
            if ( !IsAngleWithinRange(currAngle, startAngle, endAngle))
            {
                flag = false;
                return false;
            }
        }
        return true;
    }
    
        
    // Helper method to check if an angle is within a specified range
    private bool IsAngleWithinRange(float angle, float start, float end)
    {
        // Normalize the angles to ensure proper comparison
        angle = NormalizeAngle(angle);
        start = NormalizeAngle(start);
        end = NormalizeAngle(end);

        // Check if the angle is within the specified range
        if (start <= end)
        {
            return angle >= start && angle <= end;
        }
        return angle >= start || angle <= end;
    }

// Helper method to normalize an angle to the range of 0-360 degrees
    private float NormalizeAngle(float angle)
    {
        if (angle < 0f)
        {
            angle %= 360f;
            angle += 360f;
        }
        else if (angle >= 360f)
        {
            angle %= 360f;
        }
        return angle;
    }
    
    
}
