using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class rotate2exis : MonoBehaviour
{
    [SerializeField] private List<Transform> axis1;
    [SerializeField] private List<Transform> axis2;
    [SerializeField] private NavMeshSurface[] surfaces;

    private bool _isRotating = false;
    private Vector3 _previousMousePos;
    private List<GameObject> navlinks = new List<GameObject>(100);
    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }  
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            _isRotating = true;
            _previousMousePos = Input.mousePosition;
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            // Find the closest point on the NavMesh to the first surface
            // Find the closest point on the NavMesh to the first surface
            Vector3 surface1Position =surfaces[0].transform.position; // Example position of the first surface
            NavMeshHit surface1Hit;
            bool foundSurface1Hit = NavMesh.SamplePosition(surface1Position, out surface1Hit, float.PositiveInfinity, NavMesh.AllAreas);
            if (foundSurface1Hit)
            {
                surface1Position = surface1Hit.position;
            }

// Find the closest point on the NavMesh to the second surface
            Vector3 surface2Position = surfaces[1].transform.position; // Example position of the second surface
            NavMeshHit surface2Hit;
            bool foundSurface2Hit = NavMesh.SamplePosition(surface2Position, out surface2Hit, float.PositiveInfinity, NavMesh.AllAreas);
            if (foundSurface2Hit)
            {
                surface2Position = surface2Hit.position;
            }
            
            // Create a new GameObject and add a NavMeshLink component
            GameObject navLinkObject = new GameObject("NavLink");
            NavMeshLink navLink = navLinkObject.AddComponent<NavMeshLink>();
            navlinks.Add(navLinkObject);

            // Set the start and end points of the navmesh link
            navLink.startPoint = surface1Position;
            navLink.endPoint =surface2Position;
            
            // Set the width and height of the navmesh link
            navLink.width =2;

            // Set the area type of the navmesh link
            navLink.area = NavMesh.AllAreas;
            for (int i = 0; i < surfaces.Length; i++) 
            {
                surfaces [i].BuildNavMesh ();    
            }   
        }

        // Rotate the object if the left mouse button is pressed down
        if (_isRotating)
        {
            if (navlinks.Count >= 1 && navlinks[navlinks.Count-1] != null)
            {
                Destroy(navlinks[navlinks.Count-1]);
            }
            Vector3 currentMousePos = Input.mousePosition;

            // Calculate the distance between the current and previous mouse positions
            float deltaX = Mathf.Abs(currentMousePos.x - _previousMousePos.x);
            float deltaY = Mathf.Abs(currentMousePos.y - _previousMousePos.y);

            // Rotate the object based on the mouse movement
            if (deltaX > deltaY)
            {
                float rotationAmount = (currentMousePos.x - _previousMousePos.x) * 0.1f;
                foreach (var obj in axis2)
                {
                    obj.Rotate(Vector3.up, rotationAmount, Space.World);
                }
            }
            else if (deltaY > deltaX)
            {
                float xRotationAmount = -(currentMousePos.y - _previousMousePos.y) * 0.1f;
                float zRotationAmount = (currentMousePos.x - _previousMousePos.x) * 0.1f;
                foreach (var obj in axis1)
                {
                    obj.Rotate(new Vector3(xRotationAmount, 0, zRotationAmount), Space.World);
                }

            }

            // Store the current mouse position as the previous mouse position for the next frame
            _previousMousePos = currentMousePos;
        }
    }
}
