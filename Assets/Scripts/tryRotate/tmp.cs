using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


public class tmp : MonoBehaviour
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
    [SerializeField] private float timeForRotateMusic = 0.3f;


    [SerializeField]
    [Range(0, 360)]
    private int clickAngle = 5;

    private float _millisecCounter;
    private float _timeCounter;
    private float _currentTime;
    private bool _isWalking;
    private bool _isMouseUp;
    private bool _finishedAdjust;
    private float _waitTimer;
    private Vector3 _lastTarget;
    [SerializeField]
    private float speed = 150f;

    private float _direction;
    

    private Vector3 _lastPosition;
    private GameObject _lastObj;

    private Vector3 OrigDir { get; set; }
    
    
    private void Awake()
    {
        // MyTransform = transform;
        // _plane = new Plane(MyTransform.up, MyTransform.position);
        _plane = new Plane(Vector3.up, Vector3.zero);
        _isMouseUp = true;
        _finishedAdjust = true;
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
            _millisecCounter = Time.time;
            _timeCounter = Time.time;
            _isMouseUp = false;
            _finishedAdjust = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            // if(!moving.GetIsWalk()) PerformCompletionOfRound();
            BuildNewNavMesh();
            GetComponent<AudioSource>().Stop();
            _isMouseUp = true;
            _finishedAdjust = false;
        }
        else if (Input.GetMouseButton(0))
        {
            RemoveAllNavMesh();
        }
        
        if (_isDragging&&(!_isMouseUp || !_finishedAdjust))
        {
            PerformCircularRotation();
        }
    }
    
    private void FixedUpdate()
    {
        var forward = Vector3.right;
        var center = Vector3.zero;
        
        for (int i = 0; i < 360; i += clickAngle)
        {
            var angleAxis = Quaternion.AngleAxis(i, Vector3.up);
            // Debug.DrawRay(center, angleAxis * forward, color, Time.deltaTime);
            var dir = angleAxis * Vector3.right;
            Debug.DrawRay(center, dir , Color.gray, Time.deltaTime);
        }

        Debug.DrawRay(center, forward, new Color(0.32f, 0.33f, 0.5f), Time.deltaTime);
        Debug.DrawRay(center, Vector3.right, Color.black, Time.deltaTime);
        Debug.DrawRay(center, OrigDir, Color.red, Time.deltaTime);
        Debug.DrawRay(center, _lastTarget, Color.green, Time.deltaTime);
    }

    private void RemoveAllNavMesh()
    {
        // print("is walking "+!moving.GetIsWalk());
            // ||!moving.GetIsWalk()
        if ((Time.time - _millisecCounter >= SeprateBetweenRotateWalk) )
        {
            _isDragging = true;
            _isRotating = true;
        }

        if (Time.time - _timeCounter >= timeForRotateMusic)
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }
    }

    private void BuildNewNavMesh()
    {
        _isRotating = false;
        _isDragging = false;
        // surfaces[0].RemoveData();
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces[i].BuildNavMesh();
        }
    }
    
    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
    
    private void PerformCircularRotation()
    {
        var currPosition = GetPointOnPlane(Input.mousePosition);
        foreach (var world in worlds)
        {
            
            if (!world.GetComponent<World>().isKlydeOn)
            { 
              var adjustedSpeed = Time.deltaTime * speed;
              var worldTransform = world.transform;
              var center = Vector3.zero;
              var up = Vector3.up;
              
              var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) ;
              
              var curTargetDir = _lastTarget;
              // rotate by that much
              var rot = Quaternion.AngleAxis(angleDelta, up);


              if ((currPosition.z - OrigDir.z) > 0 || (currPosition.x - OrigDir.x) > 0) _direction = -1;
              else _direction = 1;
              angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);
              
              Debug.DrawLine(center, currPosition, Color.green);
              Debug.DrawRay(center, OrigDir, Color.red);
              worldTransform.Rotate(0,angleDelta,0,Space.World);  
              var absDelta = Mathf.Abs(angleDelta);
              if (absDelta < adjustedSpeed)
              {
                  _finishedAdjust = true;
                  OrigDir = _lastTarget;
                  _lastPosition = GetPointOnPlane(Input.mousePosition);
                  _lastTarget = GetClosestClick(_lastPosition - center);
                  var origDirAngleDelta = Vector3.SignedAngle(OrigDir, _lastTarget, up) % 360f;
                  origDirAngleDelta = Mathf.Clamp(origDirAngleDelta, -clickAngle, clickAngle);
                  OrigDir = Quaternion.AngleAxis(-origDirAngleDelta, up) * _lastTarget;
                  _waitTimer = 0f;

                  if (!(absDelta > 0) || _isMouseUp)
                  {
                      var fixedRot = world.rotation.eulerAngles;
                      fixedRot = new Vector3(fixedRot.x, Mathf.RoundToInt(fixedRot.z), fixedRot.y);
                      world.localRotation = Quaternion.Euler(fixedRot);
                      return;
                  }

                  adjustedSpeed -= absDelta;
                  curTargetDir = _lastTarget;
                  angleDelta = Vector3.SignedAngle(OrigDir, curTargetDir, up) % 360f;
                  angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);
                  worldTransform.Rotate(0,angleDelta,0,Space.World); 
                  rot = Quaternion.AngleAxis(angleDelta, up);
                  // world.rotation *= rot;
                  OrigDir = rot * OrigDir;

              }
              else
              {
                  _finishedAdjust = false;
                  OrigDir = rot * OrigDir;
              }
              
            }
            
        }
        // OrigDir = currPosition;
    } 
    
    private Vector3 GetClosestClick(Vector3 curDir)
    {
        var globalRight = Vector3.right;
        var up = Vector3.up;

        var curAngle = Vector3.SignedAngle(curDir, globalRight, up) % 360f;
        var closestFiveAngle = Mathf.RoundToInt(curAngle / clickAngle) * clickAngle % 360;
        var closestFiveDeg = Quaternion.AngleAxis(-closestFiveAngle, up) * globalRight;
        return closestFiveDeg;
    }
    
    
    // private void PerformCompletionOfRound()
    // {
    //     var currPosition = GetPointOnPlane(Input.mousePosition); 
    //     foreach (var world in worlds)
    //     {
    //         if (!world.GetComponent<World>().isKlydeOn)
    //         {
    //             var rotate = world.eulerAngles.y % 360;
    //             var addToRotate= rotate % 20;
    //             print(addToRotate);
    //             // var target = new Vector3(0, 0,rotate + addToRotate );
    //             // //new Vector3(world.eulerAngles.x,world.eulerAngles.z,world.eulerAngles.y);
    //             // target += world.eulerAngles;
    //             var adjustedSpeed = Time.deltaTime * speed;
    //             var worldTransform = world.transform;
    //             var center = Vector3.zero;
    //             var up = Vector3.up;
    //           
    //             var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) ;
    //             addToRotate = Mathf.Clamp(addToRotate, -adjustedSpeed, adjustedSpeed);
    //             // var targetRot = Vector3.Lerp(new Vector3(0,_direction*addToRotate,0), Vector3.zero, adjustedSpeed);
    //             print("addToRotate" + addToRotate);
    //             worldTransform.Rotate( new Vector3(0,_direction*addToRotate,0), Space.World);  
    //         }
    //         
    //     }
    //     OrigDir = currPosition;
    // } 
    
    private Vector3 GetPointOnPlane(Vector3 mousePos)
    {
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
    
}
