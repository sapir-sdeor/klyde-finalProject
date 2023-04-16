using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleRotate : MonoBehaviour
{
    [SerializeField] private Transform axis1;
    [SerializeField] private Transform axis2;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private float _currentX;
    private float _currentY;
    private bool _isRotating;
    private Vector3 _previousMousePos;
    public Transform pivotPoint;
    private GameObject centerObject;
    private float currentRotation = 0.0f;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isRotating = true;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isRotating = false;
        }
        
        if (_isRotating)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;
            float deltaX = mouseDelta.x;
            float deltaY = mouseDelta.y;
            if (Math.Abs(Mathf.Abs(deltaX) - Mathf.Abs(deltaY)) < 0.01) return;
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                axis1.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaX * 0.5f);
            else
                axis2.RotateAround(pivotPoint.position, Vector3.right, rotationSpeed * deltaY * 0.5f);
        }
    }

}
