using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class moving : MonoBehaviour
{
    private static bool isWalk;
    private static bool isWalkAnimation;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 pos;
    private static bool changeRot;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    [SerializeField] private float buffer=0.5f;
    [SerializeField] private float bufferDistance =1.5f;
    


    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // print(transform.position.x+"klyde pos");
        if (UIButtons.isPause) return;
        if (Input.GetMouseButtonDown(0)) 
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
           if (Physics.Raycast(ray, out RaycastHit raycastHit))
           {
               // print("is walking");
               isWalk = true;
               if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating()) // Check if agent is on NavMesh
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
           else isWalk = false;
        }
        // isWalk = agent.velocity.magnitude > 0.01f;
        if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating()) 
            isWalkAnimation = agent.remainingDistance > bufferDistance;
   

        if (Rotate2D3D.GetIsRotating())
        { 
           isWalkAnimation = false;
           agent.enabled = false;
           agent.velocity = Vector3.zero;
           agent.transform.position = pos;
        }
        else
        {
           pos = agent.transform.position;
           agent.enabled = true;
        }
        animator.SetBool(IsWalking,isWalkAnimation);
    }
    public static bool GetIsWalk()
    {
        return isWalk;
    }

    public static void SetWalkAnimationFalse()
    {
        isWalkAnimation = false;
    }
    
    
}

