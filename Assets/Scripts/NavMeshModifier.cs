using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshModifier : MonoBehaviour
{
    /*public float cancelThreshold = 0f;

    private NavMeshSurface navMeshSurface;
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    private int area;

    private void OnEnable()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        NavMeshModifier navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshSurface.RemoveData();
        navMeshModifier.area = NavMesh.GetAreaFromName("Not Walkable");
        navMeshSurface.BuildNavMesh();
        navMeshData = navMeshSurface.navMeshData;
        navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    private void Update()
    {
        NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();

        NavMeshData newNavMeshData = new NavMeshData(navMeshDataInstance.id);
        newNavMeshData.position = navMeshData.position;
        newNavMeshData.rotation = navMeshData.rotation;
        newNavMeshData.vertices = triangulatedNavMesh.vertices;
        newNavMeshData.indices = triangulatedNavMesh.indices;
        newNavMeshData.areas = new int[triangulatedNavMesh.vertices.Length];

        for (int i = 0; i < triangulatedNavMesh.vertices.Length; i++)
        {
            if (newNavMeshData.vertices[i].x < cancelThreshold)
            {
                newNavMeshData.areas[i] = NavMesh.GetAreaFromName("Not Walkable");
            }
            else
            {
                newNavMeshData.areas[i] = NavMesh.GetAreaFromName("Walkable");
            }
        }

        navMeshDataInstance.Remove();
        navMeshDataInstance = NavMesh.AddNavMeshData(newNavMeshData);
        navMeshSurface.RemoveData();
        navMeshSurface.navMeshData = navMeshDataInstance.navMeshData;
        navMeshSurface.BuildNavMesh();
    }*/
}
