using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NewNavLink : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] surfaces;
    [SerializeField] private float limitDistance = 10;
    [SerializeField] private float width = 5;
    private List<GameObject> navlinks = new List<GameObject>(100);
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    

    private static bool createNavLink = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("update");
        if (!createNavLink)
        {
            print("create nav link");
            // Find the closest point on the NavMesh to the first surface
            createNavLink = true;
            Vector3 surface1Position = surfaces[0].transform.position; // Example position of the first surface
            NavMeshHit surface1Hit;
            bool foundSurface1Hit = NavMesh.SamplePosition(surface1Position, out surface1Hit, float.PositiveInfinity,
                NavMesh.AllAreas);
            if (foundSurface1Hit)
            {
                print("foundSurface1Hit");
                surface1Position = surface1Hit.position;
            }

            // Find the closest point on the NavMesh to the second surface
            Vector3 surface2Position = surfaces[1].transform.position; // Example position of the second surface
            NavMeshHit surface2Hit;
            bool foundSurface2Hit = NavMesh.SamplePosition(surface2Position, out surface2Hit, float.PositiveInfinity,
                NavMesh.AllAreas);
            if (foundSurface2Hit)
            {
                print("foundSurface2Hit");
                surface2Position = surface2Hit.position;
            }

            float distance = Vector3.Distance(surfaces[0].transform.position, surfaces[1].transform.position);
            print("distance "+ distance);
            if (distance <= limitDistance)
            {
                print("limit distance is ok");
                // Create a new GameObject and add a NavMeshLink component
                GameObject navLinkObject = new GameObject("NavLink");
                NavMeshLink navLink = navLinkObject.AddComponent<NavMeshLink>();
                navlinks.Add(navLinkObject);

                // Set the start and end points of the navmesh link
                navLink.startPoint = surface1Position;
                navLink.endPoint = surface2Position;

                // Set the width and height of the navmesh link
                navLink.width = width;
                // Set the area type of the navmesh link
                navLink.area = NavMesh.AllAreas;
            }
        }
    }
    
    public static void BuildNavMesh()
    {
        createNavLink = false;
        print("build new nav link");
    }
}
