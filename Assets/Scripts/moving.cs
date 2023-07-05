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
    private static bool _stopWalk;
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
        animator.SetBool(IsWalking,isWalkAnimation);
        print("door.getWin " + Door.GetWin());
        // if (Door.GetWin())
        // {
        //     print("win is true, iswalkanimation = false "+isWalkAnimation);
        //     isWalkAnimation = false;
        // }
        // print(transform.position.x+"klyde pos");
        if (UIButtons.isPause) return;
        if (Input.GetMouseButtonDown(0)) 
        { 
            if(agent.isOnNavMesh) agent.SetDestination(transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           Debug.DrawRay(ray.origin, ray.direction * 300, Color.red);
           if (Physics.Raycast(ray, out RaycastHit raycastHit))
           {
               // print("is walking");
               isWalk = true;
               if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating()&& !_stopWalk) // Check if agent is on NavMesh
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

                   if (!Door.GetWin())
                   {
                       agent.SetDestination(target);
                   }
               }
           }
           // else isWalk = false;
        }
        // isWalk = agent.velocity.magnitude > 0.01f;
        if (!Door.GetWin())
        {
            if ( !Rotate2D3D.GetIsRotating() && agent.isOnNavMesh)
            {
                print("walk animation is true");
                isWalkAnimation = agent.remainingDistance > bufferDistance;
            } 
        
            if (Rotate2D3D.GetIsRotating())
            {
                if (agent.isOnNavMesh )
                {
                    agent.SetDestination(transform.position); 
                }
                isWalkAnimation = false;
                agent.enabled = false;
                agent.velocity = Vector3.zero;
                // agent.transform.position = pos;
            }
            else if(!Rotate2D3D.GetIsRotating()&& !isWalkAnimation )
            {
                // pos = agent.transform.position;
                agent.enabled = true;
            } 
        }
        print("isWalkAnimation "+isWalkAnimation);
        if (isWalkAnimation)
        {
            if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
        }
        else GetComponent<AudioSource>().Stop();
       
    }
    public static bool GetIsWalk()
    {
        return isWalkAnimation;
    }

    public static void SetWalkAnimationFalse()
    {
        isWalkAnimation = false;
    }
    
    public static void SetWalkAnimationTrue()
    {
        isWalkAnimation = true;
    }
    
    public static void SetStopWalk(bool val)
    {
        _stopWalk = val;
    }
    
    
}

