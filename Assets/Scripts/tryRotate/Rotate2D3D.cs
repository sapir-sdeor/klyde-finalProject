using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Rotate2D3D : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
   
    [SerializeField] private Transform[] worlds;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    private bool _isRotating;
    public Transform pivotPoint;
    private Vector2 lastMousePosition;
    private Vector2 mouseDirection;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    private List<GameObject> navlinks = new List<GameObject>(100);
    private NavMeshData navMeshData;
    private List<NavMeshDataInstance> navMeshDataInstance = new List<NavMeshDataInstance>();
    private NavMeshDataInstance _navMeshDataInstance;
    private NavMeshObstacle obstacle;
    private bool _isDragging = false;
    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();
            navMeshData = surfaces[i].navMeshData;
            _navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
            /*GameObject obstacleObj = new GameObject("Obstacle");
            obstacle = obstacleObj.AddComponent<NavMeshObstacle>();*/
        }  
    }
    
    private void Update()
    {
        // if (Mouse.current.leftButton.wasPressedThisFrame&& !moving.GetIsWalk())
        if (Input.GetMouseButton(0)&& !moving.GetIsWalk())
        {
            lastMousePosition = Input.mousePosition;
            _isRotating = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }
        if (_isRotating)
        {
            // Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;
            // float deltaX = mouseDelta.x;
            // float deltaY = mouseDelta.y;
            // foreach (var world in worlds)
            // {
            //     if (world.GetComponent<World>().isKlydeOn) continue;
            //     var position = pivotPoint.position;
            //     world.RotateAround(position, Vector3.up, rotationSpeed * deltaX * 0.5f);
            //     world.RotateAround(position, Vector3.up, rotationSpeed * deltaY * 0.5f);
            // }
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
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // GetComponent<AudioSource>().Play();
        print("begin drag");
        //make sure that navmesh will not work during the rotation
        NavMesh.RemoveAllNavMeshData();
        // NavMesh.RemoveNavMeshData(_navMeshDataInstance);
        // print("is rotate");
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            print("end drag");
            // print("build navmesh");
            surfaces[i].BuildNavMesh();
            navMeshData = surfaces[i].navMeshData;
        }
    }
}
