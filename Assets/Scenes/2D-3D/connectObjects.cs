using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectObjects : MonoBehaviour
{
    
    public GameObject object1;
    public GameObject object2;
    public float distanceThreshold = 2f;

    private GameObject surfaceObject;

    void Update()
    {
        // Calculate the distance between the two objects
        float distance = Vector3.Distance(object1.transform.position, object2.transform.position);

        if (distance <= distanceThreshold)
        {
            if (surfaceObject == null)
            {
                // Get the MeshFilters of both objects
                MeshFilter meshFilter1 = object1.GetComponent<MeshFilter>();
                MeshFilter meshFilter2 = object2.GetComponent<MeshFilter>();

                // Get the meshes of both objects
                Mesh mesh1 = meshFilter1.mesh;
                Mesh mesh2 = meshFilter2.mesh;

                // Combine the meshes into a new mesh
                Mesh newMesh = new Mesh();
                CombineInstance[] combine = new CombineInstance[2];
                combine[0].mesh = mesh1;
                combine[0].transform = meshFilter1.transform.localToWorldMatrix;
                combine[1].mesh = mesh2;
                combine[1].transform = meshFilter2.transform.localToWorldMatrix;
                newMesh.CombineMeshes(combine);

                // Calculate the triangles that form the surface between the two objects
                int[] triangles = newMesh.triangles;
                List<int> surfaceTriangles = new List<int>();
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int vertexIndex1 = triangles[i];
                    int vertexIndex2 = triangles[i + 1];
                    int vertexIndex3 = triangles[i + 2];

                    Vector3 vertex1 = newMesh.vertices[vertexIndex1];
                    Vector3 vertex2 = newMesh.vertices[vertexIndex2];
                    Vector3 vertex3 = newMesh.vertices[vertexIndex3];

                    // Check if any of the vertices is within the threshold distance of the other object
                    if (Vector3.Distance(vertex1, object2.transform.position) <= distanceThreshold ||
                        Vector3.Distance(vertex2, object2.transform.position) <= distanceThreshold ||
                        Vector3.Distance(vertex3, object2.transform.position) <= distanceThreshold)
                    {
                        surfaceTriangles.Add(vertexIndex1);
                        surfaceTriangles.Add(vertexIndex2);
                        surfaceTriangles.Add(vertexIndex3);
                    }
                }

                // Create a new mesh that contains only the surface triangles
                Mesh surfaceMesh = new Mesh();
                surfaceMesh.vertices = newMesh.vertices;
                surfaceMesh.triangles = surfaceTriangles.ToArray();

                // Assign the new mesh to a new GameObject with a MeshFilter and MeshRenderer
                surfaceObject = new GameObject("Surface");
                MeshFilter surfaceMeshFilter = surfaceObject.AddComponent<MeshFilter>();
                surfaceMeshFilter.mesh = surfaceMesh;
                MeshRenderer surfaceMeshRenderer = surfaceObject.AddComponent<MeshRenderer>();
                surfaceMeshRenderer.material = meshFilter1.GetComponent<MeshRenderer>().sharedMaterial;

                // Attach the new mesh to the first object's transform
                surfaceObject.transform.SetParent(object1.transform, false);
                // Add a MeshCollider to the new mesh for collision detection
             //   surfaceObject.AddComponent<MeshCollider>();
            }
        }
        else
        {
            if (surfaceObject != null)
            {
                // Destroy the surface object and the mesh collider
                Destroy(surfaceObject.GetComponent<MeshCollider>());
                Destroy(surfaceObject);
                surfaceObject = null;
            }
        }
    }
}
