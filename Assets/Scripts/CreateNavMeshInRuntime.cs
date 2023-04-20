using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshInRuntime : MonoBehaviour
{
  
    [SerializeField] private NavMeshSurface[] surfaces;


    void Update () 
    {

        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }
    
}
