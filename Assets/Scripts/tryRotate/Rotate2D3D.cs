using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate2D3D : MonoBehaviour
{
    [SerializeField] private Transform axis1;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private bool _isRotating;
    public Transform pivotPoint;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !axis1.GetComponent<World>().isKlydeOn)
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
            axis1.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaX * 0.5f);

        }
    }
}
