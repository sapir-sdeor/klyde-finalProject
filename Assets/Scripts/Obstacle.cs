using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    // private NavMeshObstacle _navMeshObstacle;
    private Transform _parentTransform;
    private Transform _childTransform;
    [SerializeField] private bool condition;

    // Update is called once per frame
    void Update()
    {
       
        //TODO: add way to calculate if its rotating now or not
        // Get references to the parent and child transforms
        var transform1 = transform;
        _parentTransform = transform1.parent;
        _childTransform = transform1;
        
        // Calculate the world position of the child transform
        Vector3 worldPosition = _parentTransform.TransformPoint(_childTransform.localPosition);
        //todo: make it general
        if (worldPosition.x >= 0)
        {
            Destroy(GetComponent<NavMeshObstacle>());
            // print("destroy navmesh obstacle");
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            if (gameObject.GetComponent<NavMeshObstacle>() == null)
            {
                gameObject.AddComponent<NavMeshObstacle>();
                // print("nav mesh obstacle");
            }
        }
    }
}
