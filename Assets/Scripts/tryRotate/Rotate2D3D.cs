using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Rotate2D3D : MonoBehaviour
{
   
    [SerializeField] private Transform axis1;
    [SerializeField] private Transform axis2;
    [SerializeField] private Transform[] axes;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private bool _isRotating;
    public Transform pivotPoint;
    
    private Vector2 lastMousePosition;
    private Vector2 mouseDirection;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    private List<GameObject> navlinks = new List<GameObject>(100);
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    private NavMeshObstacle obstacle;
    private double start_time = 0;
    void Start()
    {
        start_time = Time.deltaTime;
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();
            navMeshData = surfaces[i].navMeshData;
            navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
            GameObject obstacleObj = new GameObject("Obstacle");
            obstacle = obstacleObj.AddComponent<NavMeshObstacle>();
        }  
    }
    
    private void Update()
    {
        var time = Time.deltaTime;
        if (Mouse.current.leftButton.wasPressedThisFrame && !axis2.GetComponent<World>().isKlydeOn)
        {
            print("rotate is true");
            lastMousePosition = Input.mousePosition;
            _isRotating = true;
        }
         // Check if the left mouse button is released
         // Stop tracking mouse movement when left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            for (int i = 0; i < surfaces.Length; i++) 
            {
                surfaces [i].BuildNavMesh ();
            }
        }
        if (_isRotating)
        {
            //make sure that navmesh will not work during the rotation
            for (int i = 0; i < surfaces.Length; i++) 
            {
                NavMesh.RemoveNavMeshData(navMeshDataInstance);
                print("clear all navmesh during rotation");
            }
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;
            float deltaX = mouseDelta.x;
            float deltaY = mouseDelta.y;
            axis2.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaX * 0.5f);
            axis2.RotateAround(pivotPoint.position, Vector3.up, rotationSpeed * deltaY * 0.5f);
        }
        if(time- start_time >= 2f){
            NavMesh.RemoveNavMeshData(navMeshDataInstance);
            print("2f");
        }
    }
}
