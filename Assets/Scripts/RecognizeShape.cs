using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizeShape : MonoBehaviour
{
    // [SerializeField] private GameObject[] positionsGameObjects;
    [SerializeField] private Row[] grid;
    [SerializeField] private GameObject objectToShown;

    [SerializeField] private GameObject rightPlane, leftPlane;
    [SerializeField] private GameObject reflect;
    [SerializeField] private float timeToDisappearLimit = 7,_soundTimeLimit=4;
    [SerializeField] private float offset=0;

    [SerializeField] private AudioSource _audioRecognize, _audioWindow;
    // Start is called before the first frame update
    private static bool _recognizeShape ;
    private float _angle;
    private bool flag = true;
    private float _timeToDisappear,_soundTime;
    private static bool _showObject,_level1SoundPlay=true,_recognizeShapeSound=true;

    private void Start()
    {
        _recognizeShape = false;
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
        if (LevelManager.GetLevel() == 1)
        {
            rightPlane.GetComponent<Animator>().SetBool("Level1",true); 
            leftPlane.GetComponent<Animator>().SetBool("Level1",true);
        }
        else
        {
            rightPlane.GetComponent<Animator>().SetBool("Level1",false); 
            leftPlane.GetComponent<Animator>().SetBool("Level1",false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        flag = true;
        if (LevelManager.GetLevel() == 1 && _level1SoundPlay)
        {
            _soundTime += Time.deltaTime;
            if (_soundTime > _soundTimeLimit)
            {
                _level1SoundPlay = false;
                if (!_audioWindow.isPlaying)
                    _audioWindow.Play();
            }
        }

        if (_recognizeShape)
        {
            _timeToDisappear += Time.deltaTime;
        }
        
        
        if (_timeToDisappear > timeToDisappearLimit)
        {
            rightPlane.GetComponent<Animator>().SetBool("recognizeShape",true); 
            leftPlane.GetComponent<Animator>().SetBool("recognizeShape",true);
            if (!_audioWindow.isPlaying && _recognizeShapeSound)
            {
                _audioWindow.Play();
                _recognizeShapeSound = false;
            }
            objectToShown.gameObject.SetActive(false);
        }

        //for multiple we have grid with row that hold two game objects and the distance between them
        foreach (var row in grid)
        {
            var dist = Vector3.Distance(row.positions[0].transform.position, row.positions[1].transform.position);
           // print(dist + " distance ," +" points in right half? name: " +  row.gameObject.name);


            if (row.distance - row.aprroximate >= dist || dist >= row.distance + row.aprroximate ||
                Rotate2D3D.GetIsRotating() || !PointsInRightHalf(row))
                flag = false;
        }
        if (flag && !_recognizeShape){
            if (LevelManager.GetLevel() != (5) && LevelManager.GetLevel() != (6) && LevelManager.GetLevel() != (7))
            {
                Rotate2D3D.AutomaticCompleteTheShape();
            }
            else
            {
                _showObject = true;
            }
             
        }

        if (_showObject)
        {
            _showObject = false;
            _recognizeShape = true;
            objectToShown.gameObject.SetActive(true);
            if (!_audioRecognize.isPlaying)
                _audioRecognize.Play();
            if(reflect)
                reflect.gameObject.SetActive(false);
        }
        
    }

    public static void SetShowObject()
    {
        _showObject = true;
    }

    public static bool GetRecognizeShape()
    {
        return _recognizeShape;
    }
    
    private bool PointsInRightHalf(Row row)
    {
        for(int i=0; i < 2 ;i++)
        {
            // print("is angle within range");
            Transform trans =row.positions[i].transform;
            Vector3 direction = trans.position - Vector3.zero;
            // Calculate the angle between the direction vector and the forward vector
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
