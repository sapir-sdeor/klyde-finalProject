using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class moving : MonoBehaviour
{
    private static bool isWalk;
    private NavMeshAgent agent;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        // if (Mouse.current.leftButton.wasReleasedThisFrame) 
        if (Input.GetMouseButtonUp(0)) 
        {
           isWalk = true;
           // print("get mouse button down");
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
           Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

           if (Physics.Raycast(ray, out RaycastHit hit))
           {
               // print(agent.isOnNavMesh);
               if (agent.isOnNavMesh) // Check if agent is on NavMesh
               {
                   // print("is on navmesh");
                   agent.SetDestination(hit.point);
               }
           }
        }
        isWalk = agent.velocity.magnitude > 0.01f;
       
       // print(agent.velocity.magnitude+ " agent vel mangititude");

       if (agent.isOnOffMeshLink)
       {
           // print("jump");
       }
       
       animator.SetBool(IsWalking,agent.velocity.magnitude > 0.01f);
    }

    public static bool GetIsWalk()
    {
        return isWalk;
    } 
 
}

