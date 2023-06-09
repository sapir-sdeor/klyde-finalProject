using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Circle : World
{
    private GameObject klyde;
    private float _angle;
    private bool _changeTexture,_changeTextureStart;
    private float _startTime;
    [SerializeField] private int halfNum;
    [SerializeField] private float timeToChangeTexture = 3.35f;
    [SerializeField] private float offset=-60;
    
    void Start()
    {
        // print(LevelManager.GetNumOfHalves());
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
        klyde = GameObject.FindGameObjectWithTag("klyde");
        _startTime = Time.time;

    }
    
    void Update()
    {
        if (LevelManager.GetLevel()==1 && Time.time - _startTime > timeToChangeTexture && !_changeTextureStart)
        {
            _changeTextureStart = true;
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.CompareTag("circle")) continue;
                child.GetComponent<MeshRenderer>().material.mainTexture = textureWithShape;
                child.GetComponent<MeshRenderer>().material.SetFloat("_Angle", _angle);
                child.GetComponent<MeshRenderer>().material.SetInt("_HalfNum", halfNum);
            }
        }
        var trans = klyde.transform;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        if (currAngle < 0) currAngle += 360f;

        // Calculate the adjusted angle range based on the number of halves
        float startAngle = _angle * (halfNum - 1)+offset;
        float endAngle = _angle * halfNum+offset;

        // Handle wraparound from 360 to 0 degrees
        if (endAngle > 360f)
            endAngle -= 360f;

        // Check if the current angle is within the adjusted range
        isKlydeOn = IsAngleWithinRange(currAngle, startAngle, endAngle);

        var parentAngle = transform.localEulerAngles.y;
        if (parentAngle < 0) parentAngle += 360;
        if (parentAngle >= 360) parentAngle += 360 - parentAngle;

        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.CompareTag("circle")) continue;
            if (RecognizeShape.GetRecognizeShape() && !_changeTexture && textureWithoutShape)
            {
                child.GetComponent<MeshRenderer>().material.mainTexture = textureWithoutShape;
                _changeTexture = true;
            }
            child.GetComponent<MeshRenderer>().material.SetFloat("_Angle", _angle);
            child.GetComponent<MeshRenderer>().material.SetInt("_HalfNum", halfNum);
        }
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

    public float GetRotation()
    {
        return gameObject.transform.eulerAngles.y;
    }

}
  
