using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moving : MonoBehaviour
{
    private NavMeshAgent agent;
    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
       if (Input.GetMouseButtonDown(0))
       {
           print("get mouse button down");
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
           Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

           if (Physics.Raycast(ray, out RaycastHit hit))
           {
               print(agent.isOnNavMesh);
               if (agent.isOnNavMesh) // Check if agent is on NavMesh
               {
                   print("is on navmesh");
                   agent.SetDestination(hit.point);
               }
           }

       }

       if (agent.isOnOffMeshLink)
       {
           print("jump");
       }
    }
 
}

