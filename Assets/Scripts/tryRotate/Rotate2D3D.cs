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
    
    private Vector2 lastMousePosition;
    private Vector2 mouseDirection;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !axis1.GetComponent<World>().isKlydeOn)
        {
            lastMousePosition = Input.mousePosition;
            _isRotating = true;
        }

        // Stop tracking mouse movement when left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }
        if (_isRotating)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;
            float deltaX = mouseDelta.x;
            float deltaY = mouseDelta.y;
            axis1.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaX * 0.5f);
            axis1.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaY * 0.5f);
        }
    }
}
