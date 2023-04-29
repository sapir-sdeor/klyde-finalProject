using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarching : MonoBehaviour
{
    public Material shape1Material;
    public Material shape2Material;
    public Material mergedMaterial;
    public int maxIterations = 100;
    public float maxDistance = 100.0f;
    public float mergeThreshold = 0.1f;
    private Mesh mergedMesh;

    void Start()
    {
        mergedMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mergedMesh;
    }

    void Update()
    {
        Vector3 shape1Position = shape1Material.GetVector("_Position");
        Quaternion shape1Rotation = Quaternion.Euler(shape1Material.GetVector("_Rotation"));
        Vector3 shape1Scale = shape1Material.GetVector("_Scale");

        Vector3 shape2Position = shape2Material.GetVector("_Position");
        Quaternion shape2Rotation = Quaternion.Euler(shape2Material.GetVector("_Rotation"));
        Vector3 shape2Scale = shape2Material.GetVector("_Scale");

        Matrix4x4 shape1Matrix = Matrix4x4.TRS(shape1Position, shape1Rotation, shape1Scale);
        Matrix4x4 shape2Matrix = Matrix4x4.TRS(shape2Position, shape2Rotation, shape2Scale);

        mergedMesh.Clear();
        mergedMesh.vertices = MergeShapes(shape1Matrix, shape2Matrix);
        mergedMesh.triangles = new int[mergedMesh.vertices.Length];
        for (int i = 0; i < mergedMesh.vertices.Length; i++)
        {
            mergedMesh.triangles[i] = i;
        }
        mergedMesh.RecalculateNormals();
    }

    Vector3[] MergeShapes(Matrix4x4 shape1Matrix, Matrix4x4 shape2Matrix)
    {
        Vector3[] vertices = new Vector3[0];

        // Merge the two shapes together using ray marching
        for (int i = 0; i < maxIterations; i++)
        {
            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = Camera.main.transform.forward;
            Vector3 p = rayOrigin + rayDirection * maxDistance;

            float d1 = SignedDistance(shape1Matrix.inverse.MultiplyPoint3x4(p), shape1Material);
            float d2 = SignedDistance(shape2Matrix.inverse.MultiplyPoint3x4(p), shape2Material);
            float blend = Mathf.SmoothStep(-mergeThreshold, mergeThreshold, d1 - d2);
            float distance = Mathf.Lerp(d1, d2, blend);

            if (distance < 0.001f)
            {
                // Hit the merged shape, add vertex
                System.Array.Resize(ref vertices, vertices.Length + 1);
                vertices[vertices.Length - 1] = Camera.main.transform.InverseTransformPoint(rayOrigin + rayDirection * distance);
            }

            rayOrigin += rayDirection * distance;
            if (distance > maxDistance) break;
        }

        return vertices;
    }

    float SignedDistance(Vector3 point, Material material)
    {
        Shader.SetGlobalVector("_Position", material.GetVector("_Position"));
        Shader.SetGlobalVector("_Rotation", material.GetVector("_Rotation"));
        Shader.SetGlobalVector("_Scale", material.GetVector("_Scale"));
        Shader.SetGlobalColor("_Color", material.color);
        return Shader.GetGlobalFloat("_SignedDistanceFunction") + material.GetFloat("_DistanceOffset");
    }

    void OnRenderObject()
    {
        mergedMaterial.SetPass(0);
        Graphics.DrawMeshNow(mergedMesh, transform.localToWorldMatrix);
    }
}


