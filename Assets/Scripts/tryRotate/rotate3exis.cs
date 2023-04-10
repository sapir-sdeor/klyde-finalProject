using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate3exis : MonoBehaviour
{
    [SerializeField] private List<Transform> axis1;
    [SerializeField] private List<Transform> axis2;
    [SerializeField] private List<Transform> axis3;
    [SerializeField] private float rotationSpeed = 2f;
    private bool _isRotating;
    private Vector3 _previousMousePos;
    private Vector3[] _initialRotationAxis2;

    private void Start()
    {
        _initialRotationAxis2 = new Vector3[axis2.Count];
        for (int i = 0; i < axis2.Count; i++)
        {
            _initialRotationAxis2[i] = axis2[i].eulerAngles;
        }
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            foreach (var obj in axis1)
            {
                obj.Rotate(new Vector3(0,rotationSpeed,0), Space.Self);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            foreach (var obj in axis2)
            {
                Vector3 euler = obj.transform.eulerAngles;
                obj.rotation = Quaternion.Euler(euler.x, euler.y + rotationSpeed, euler.z);
            }
        }
        
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            foreach (var obj in axis3)
            {
                obj.Rotate(new Vector3(0,rotationSpeed,0), Space.Self);
            }
        }
    }
}
