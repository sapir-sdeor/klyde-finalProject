using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshLinkInRuntime : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] surfaces;
    private List<GameObject> navlinks = new List<GameObject>(100);
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    private bool _isRotating;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
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
            
            float distance = Vector3.Distance(surfaces[0].transform.position, surfaces[1].transform.position);
            if (distance <= 5f) {
                print("create navlink");
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
            }
        }
    }
}
