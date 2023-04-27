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
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float sensitivity1 = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float rotationSpeed1 = 2f;
    private bool _isRotating;
    public Transform pivotPoint;
    
    private Vector2 lastMousePosition;
    private Vector2 mouseDirection;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    private List<GameObject> navlinks = new List<GameObject>(100);
    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }  
    }
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !axis1.GetComponent<World>().isKlydeOn)
        {
            lastMousePosition = Input.mousePosition;
            _isRotating = true;
        }
         // Check if the left mouse button is released
         // Stop tracking mouse movement when left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            // Find the closest point on the NavMesh to the first surface
            // Find the closest point on the NavMesh to the first surface
//             Vector3 surface1Position =surfaces[0].transform.position; // Example position of the first surface
//             NavMeshHit surface1Hit;
//             bool foundSurface1Hit = NavMesh.SamplePosition(surface1Position, out surface1Hit, float.PositiveInfinity, NavMesh.AllAreas);
//             if (foundSurface1Hit)
//             {
//                 surface1Position = surface1Hit.position;
//             }
//
// // Find the closest point on the NavMesh to the second surface
//             Vector3 surface2Position = surfaces[1].transform.position; // Example position of the second surface
//             NavMeshHit surface2Hit;
//             bool foundSurface2Hit = NavMesh.SamplePosition(surface2Position, out surface2Hit, float.PositiveInfinity, NavMesh.AllAreas);
//             if (foundSurface2Hit)
//             {
//                 surface2Position = surface2Hit.position;
//             }
//             
//             float distance = Vector3.Distance(surfaces[0].transform.position, surfaces[1].transform.position);
//             if (distance <= 5f) {
//                 print("create navlink");
//                 // Create a new GameObject and add a NavMeshLink component
//                 GameObject navLinkObject = new GameObject("NavLink");
//                 NavMeshLink navLink = navLinkObject.AddComponent<NavMeshLink>();
//                 navlinks.Add(navLinkObject);
//
//                 // Set the start and end points of the navmesh link
//                 navLink.startPoint = surface1Position;
//                 navLink.endPoint =surface2Position;
//             
//                 // Set the width and height of the navmesh link
//                 navLink.width =2;
//
//                 // Set the area type of the navmesh link
//                 navLink.area = NavMesh.AllAreas;
//             }
          
            for (int i = 0; i < surfaces.Length; i++) 
            {
                surfaces [i].BuildNavMesh ();    
            }
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
