using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Rotate2D3D : MonoBehaviour
{
   
    [SerializeField] private Transform[] worlds;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private static bool _isRotating;
    private static bool _isDragging;
    public Transform pivotPoint;
    private Vector2 lastMousePosition;
    private Vector2 mouseDirection;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    private int frameCounter;
    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh (); ;
        }  
    }
    
    private void Update()
    {
        // if (Mouse.current.leftButton.wasPressedThisFrame&& !moving.GetIsWalk())
        if (Input.GetMouseButton(0)&& !moving.GetIsWalk())
        {
            RemoveAllNavMesh();
        }

        if (Input.GetMouseButtonDown(0))
        {
            frameCounter = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            BuildNewNavMesh();
        }
        if (_isRotating)
        {
            RotateWorld();
        }
    }

    private void RemoveAllNavMesh()
    {
        lastMousePosition = Input.mousePosition;
        _isRotating = true;
        frameCounter++;
        if (frameCounter >= 20)
        {
            NavMesh.RemoveAllNavMeshData();
            _isDragging = true;
            // print("drag");
        }
    }

    private void BuildNewNavMesh()
    {
        // print("button up");
        _isRotating = false;
        _isDragging = false;
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces[i].BuildNavMesh();
        }
    }

    private void RotateWorld()
    {
        // Get the amount of mouse movement since the last frame
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        // Multiply by sensitivity to get desired movement
        deltaX *= sensitivity;
        deltaY *= sensitivity;

        // Rotate the worlds around the pivot point based on mouse movement
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;
            var position = pivotPoint.position;
            world.transform.RotateAround(position, Vector3.up, rotationSpeed * deltaX * 0.5f);
            world.transform.RotateAround(position, Vector3.up, rotationSpeed * deltaY * 0.5f);
        } 
    }

    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
}
