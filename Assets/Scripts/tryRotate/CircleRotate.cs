using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotate : MonoBehaviour
{
    [SerializeField] private List<Transform> axis1;
    [SerializeField] private List<Transform> axis2;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private float _currentX;
    private float _currentY;
    private bool _isRotating;
    private Vector3 _previousMousePos;
    private Vector3[] _initialRotationAxis2;

    private void Start()
    {
        _initialRotationAxis2 = new Vector3[axis2.Count];
        for (int i = 0; i < axis2.Count; i++)
        {
            _initialRotationAxis2[i] = axis2[i].eulerAngles;
            print(_initialRotationAxis2[i].z);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isRotating = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }
        
        if (_isRotating)
        {
            /*float deltaX = Input.GetAxis("Mouse X") * sensitivity;
            currentX += deltaX;
            transform.rotation = Quaternion.Euler(0, currentX, 0);*/
            float deltaX = Input.GetAxis("Mouse X") * sensitivity;
            float deltaY = Input.GetAxis("Mouse Y") * sensitivity;

            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                _currentX += deltaX;
                foreach (var obj in axis1)
                {
                    obj.rotation = Quaternion.Euler(0, _currentX * rotationSpeed, 0);
                }
                
            }
            else
            {
                _currentY += deltaY;
                for (int i = 0; i < axis2.Count; i++)
                {
                    axis2[i].rotation = Quaternion.Euler(_currentY * rotationSpeed, _initialRotationAxis2[i].y,
                        _initialRotationAxis2[i].z);
                }
                
            }
        }
    }

}
