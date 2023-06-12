using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


public class Rotate2D3D : MonoBehaviour
{
    [SerializeField] private Transform[] worlds;
    private static bool _isRotating ;
    private static bool _isDragging ;
    private Vector3 lastMousePosition;
    private Vector3 lastPosition;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    // [SerializeField] private NavMeshLink linkObject1,linkObject2;
    private Plane _plane;
    [SerializeField] private float SeprateBetweenRotateWalk;
    private float millisecCounter;
    private float currentTime;
    private bool isWalking;
    [SerializeField]
    private float speed = 150f;
    

    private Vector3 _lastPosition;
    private GameObject _lastObj;

    private Vector3 OrigDir { get; set; }
    
    
    private void Awake()
    {
        // MyTransform = transform;
        // _plane = new Plane(MyTransform.up, MyTransform.position);
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    
    void Start()
    {
        BuildNewNavMesh();
        // Disable VSync
        QualitySettings.vSyncCount = 0;
        // Set the desired frame rate
        Application.targetFrameRate = 60;
    }
    
    private void Update()
    {
        if (Door.moveToVitraje) return;
        // if (Mouse.current.leftButton.wasPressedThisFrame&& !moving.GetIsWalk())
        if (UIButtons.isPause)
        {
            GetComponent<AudioSource>().Stop();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            OrigDir = _lastPosition;
            millisecCounter = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            BuildNewNavMesh();
            GetComponent<AudioSource>().Stop();
        }
        else if (Input.GetMouseButton(0))
        {
            RemoveAllNavMesh();
        }
        
        if (_isDragging)
        {
            PerformCircularRotation();
        }
    }

    private void RemoveAllNavMesh()
    {
        print("is walking "+!moving.GetIsWalk());
            // ||!moving.GetIsWalk()
        if ((Time.time - millisecCounter >= SeprateBetweenRotateWalk) )
        {
            _isDragging = true;
            _isRotating = true;
        }
    }

    private void BuildNewNavMesh()
    {
        _isRotating = false;
        _isDragging = false;
        // surfaces[0].RemoveData();
        for (int i = 0; i < surfaces.Length; i++) 
        {
            // NavMeshLink navMeshLink = linkObject.GetComponent<NavMeshLink>();
            // linkObject1.UpdateLink();
            // linkObject2.UpdateLink();
            surfaces[i].BuildNavMesh();
        }
        print("build new navmesh");
        // if (!moving.GetIsWalk())
        // {
        // }
    }
    
    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
    
    private void PerformCircularRotation()
    {
        print(worlds.Length + "world length");
        var currPosition = GetPointOnPlane(Input.mousePosition);
        foreach (var world in worlds)
        {
            
            print(world.name + " world name is klyde on "+world.GetComponent<World>().isKlydeOn);
            if (!world.GetComponent<World>().isKlydeOn)
            { 
              var adjustedSpeed = Time.deltaTime * speed;
              var worldTransform = world.transform;
              var center = Vector3.zero;
              var up = Vector3.up;
              
              var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) ;
              print(world.name + " world name angle delta "+angleDelta); 
              // angleDelta = HandleFlipAngle(center, currPosition, up, angleDelta);
              // TODO: Any smoothing will probably go here
              // rotate by that much
              //var rot = Quaternion.AngleAxis(angleDelta * 1f, up);
              //OrigDir = rot * OrigDir;
              
              angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);
              
              Debug.DrawLine(center, currPosition, Color.green);
              Debug.DrawRay(center, OrigDir, Color.red);
              worldTransform.Rotate(0,angleDelta,0,Space.World);  
            }
            
        }
        OrigDir = currPosition;
    } 
    
    private Vector3 GetPointOnPlane(Vector3 mousePos)
    {
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
    
}
