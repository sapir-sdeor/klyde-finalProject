using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryConnectMeshes : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject1;
    [SerializeField] private GameObject _gameObject2;
    private Mesh _mesh1;
    private Mesh _mesh2;
    private bool did;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (did) return;
        GameObject combinedObj = new GameObject("Combined Mesh");

        // Get the mesh filter and renderer components of the new object
        MeshFilter meshFilter = combinedObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = combinedObj.AddComponent<MeshRenderer>();

        // Combine the meshes of the two objects
        Mesh combinedMesh = new Mesh();
        CombineInstance[] combineInstances = new CombineInstance[2];

        combineInstances[0].mesh = _gameObject1.GetComponent<MeshFilter>().sharedMesh;
        combineInstances[0].transform = _gameObject1.transform.localToWorldMatrix;

        combineInstances[1].mesh = _gameObject2.GetComponent<MeshFilter>().sharedMesh;
        combineInstances[1].transform = _gameObject2.transform.localToWorldMatrix;

        combinedMesh.CombineMeshes(combineInstances);

        // Set the combined mesh to the mesh filter
        meshFilter.sharedMesh = combinedMesh;

        // Copy the materials from the first object to the renderer
        meshRenderer.sharedMaterials = _gameObject1.GetComponent<MeshRenderer>().sharedMaterials;

        var filter = _gameObject1.GetComponent<MeshFilter>();
        filter.sharedMesh = combinedMesh;

        Vector3 position = combinedObj.transform.position;
        Quaternion rotation = combinedObj.transform.rotation;
        Vector3 scale = combinedObj.transform.localScale;

        Transform targetTransform = _gameObject1.transform;
        targetTransform.position = position;
        targetTransform.rotation = rotation;
        Vector3 parentScale = targetTransform.parent != null ? targetTransform.parent.lossyScale : Vector3.one;
        targetTransform.localScale = new Vector3(scale.x / parentScale.x, scale.y / parentScale.y, scale.z / parentScale.z);
        did = true;


    }
}
