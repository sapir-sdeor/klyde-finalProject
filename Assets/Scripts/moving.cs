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
    [SerializeField] private float buffer=0.5f;

    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // print(transform.position.x+"klyde pos");
        if (Input.GetMouseButtonDown(0)) 
        {
           // isWalk = true;
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
           // print(Physics.Raycast(ray, out RaycastHit raycast));
           if (Physics.Raycast(ray, out RaycastHit raycastHit))
           {
               // print(!Rotate2D3D.GetIsRotating() +" is not rotate?");
               if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating() ) // Check if agent is on NavMesh
               {
                   var target = raycastHit.point;
                   if (-buffer <= target.x && target.x <= +buffer)
                   {
                       if (target.x > 0)
                       {
                           target.x += buffer;
                       }
                       else
                       {
                           target.x -= buffer;
                       }
                   }
                   agent.SetDestination(target);
               }
           }
        }
        isWalk = agent.velocity.magnitude > 0.01f;

       if (Rotate2D3D.GetIsRotating())
       {
           agent.enabled = false;
           agent.velocity = Vector3.zero;
           agent.transform.position = pos;
       }
       else
       {
           pos = agent.transform.position;
           agent.enabled = true;
       }
       
       animator.SetBool(IsWalking,isWalk);
    }

    public static bool GetIsWalk()
    {
        return isWalk;
    } 
    
    // if (!Rotate2D3D.GetIsRotating())
    // {
    //     Vector3 direction = (raycastHit.point - transform.position).normalized;
    //     float desiredSpeed =  speed * 0.5f;
    //     Vector3 desiredVelocity = direction * speed;
    //     if (Vector3.Distance(transform.position, raycastHit.point) <= stoppingDistance)
    //     {
    //         rb.velocity = Vector3.zero;
    //     }
    //     else
    //     {
    //         rb.velocity = desiredVelocity;
    //     }
    // }
 
}

