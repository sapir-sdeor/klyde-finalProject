using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshInRuntime : MonoBehaviour
{
  
    [SerializeField] private NavMeshSurface[] surfaces;
    [SerializeField] private Transform[] objectsToRotate;


    void Update () 
    {

        for (int j = 0; j < objectsToRotate.Length; j++) 
        {
            objectsToRotate [j].localRotation = Quaternion.Euler (new Vector3 (0, 10*Time.deltaTime, 0)+objectsToRotate[j].localRotation.eulerAngles);
        }

        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }
    
}
