using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class moving : MonoBehaviour
{
    private static bool isWalk;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 pos;
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
        if (Input.GetMouseButtonDown(0)) 
        {
           isWalk = true;
           // print("get mouse button down");
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
           Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);

           if (Physics.Raycast(ray, out RaycastHit hit))
           {
               print(Rotate2D3D.GetIsRotating());
               if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating()) // Check if agent is on NavMesh
               {
                   agent.SetDestination(hit.point);
               }
           }
        }
        isWalk = agent.velocity.magnitude > 0f;
        // if (agent.isOnOffMeshLink)
       // {
       //     // print("jump");
       // }
       if (Rotate2D3D.GetIsRotating())
       {
           agent.velocity = Vector3.zero;
           agent.transform.position = pos;
       }
       else
       {
           pos = agent.transform.position;
       }
       
       animator.SetBool(IsWalking,isWalk);
    }

    public static bool GetIsWalk()
    {
        return isWalk;
    } 
 
}

