using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;



public class Rotate2D3D : MonoBehaviour
{
    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private Transform[] worlds;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float rotationSpeed = 1f;
    private static bool _isRotating ;
    private static bool _isDragging ;
    public Transform pivotPoint;
    private Vector3 lastMousePosition;
    private Vector3 _pos;
    private Vector3 _mousePosStart;
    private Vector3 lastPosition;
    
    [SerializeField] private NavMeshSurface[] surfaces;
    [SerializeField] private float SeprateBetweenRotateWalk;
    [SerializeField] private float maxRotationAmount = 10f;
    private float millisecCounter;
    private float currentTime;
    private bool isWalking;
    [SerializeField]
    private float speed = 150f;

    [SerializeField]
    private float flipAngle = 30f;

    [SerializeField]
    private float flipDelta = 10f;

    [SerializeField]
    private bool createObj = true;

    [SerializeField]
    private GameObject obj;

    private Vector3 _lastPosition;
    private GameObject _lastObj;
    private Plane _plane;

    private Vector3 OrigDir { get; set; }

    private Transform MyTransform { get; set; }
    
    private void Awake()
    {
        // MyTransform = transform;
        // _plane = new Plane(MyTransform.up, MyTransform.position);
        _plane = new Plane(worlds[1].transform.up, worlds[1].transform.position);
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
        // if (Mouse.current.leftButton.wasPressedThisFrame&& !moving.GetIsWalk())
        if (UIButtons.isPause)
        {
            GetComponent<AudioSource>().Stop();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosStart = Input.mousePosition;
            Vector3 localPos = new Vector3(worlds[0].transform.parent.transform.position.x, 0,
                worlds[0].transform.parent.transform.position.z);
            _pos = _mousePosStart - localPos;
            _lastPosition = GetPointOnPlane(Input.mousePosition,worlds[1].transform);
            // OrigDir = _lastPosition - MyTransform.position;
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
            // RotateWorld();
            PerformCircularRotation();
        }
    }

    private void RemoveAllNavMesh()
    {
        if (Time.time - millisecCounter >= SeprateBetweenRotateWalk)
        {
            _isDragging = true;
            _isRotating = true;
        }
    }

    private void BuildNewNavMesh()
    {
        _isRotating = false;
        _isDragging = false;
        if (!moving.GetIsWalk())
        {
            for (int i = 0; i < surfaces.Length; i++) 
            {
                surfaces[i].BuildNavMesh();
            } 
        }
    }
    
    public static bool GetIsRotating()
    {
        return _isRotating;
    }
  
    
    private void PerformCircularRotation()
    {
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;
            var adjustedSpeed = Time.deltaTime * speed;
            var worldTransform = world.transform;
            var center = Vector3.zero;
            // Vector3 center = new Vector3(worlds[0].transform.parent.transform.position.x, 0,
            //     worlds[0].transform.parent.transform.position.z);
            // var center = Vector3.zero;
            var up = Vector3.up;
            var rotation = worldTransform.rotation;

            var currPosition = GetPointOnPlane(Input.mousePosition,worldTransform);
            

            var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) ;
            // angleDelta = HandleFlipAngle(center, currPosition, up, angleDelta);
            // TODO: Any smoothing will probably go here
            // rotate by that much
            //var rot = Quaternion.AngleAxis(angleDelta * 1f, up);
            //OrigDir = rot * OrigDir;
            OrigDir = currPosition;
            angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);

            Debug.DrawLine(center, currPosition, Color.green);
            Debug.DrawRay(center, OrigDir, Color.red);

            
            worldTransform.Rotate(0,angleDelta,0,Space.World);
            // worldTransform.rotation = tmpRot;
            // worldTransform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, tmpRot.eulerAngles.y, world.transform.rotation.eulerAngles.z);;
        }
        
    } 
    
    private float HandleFlipAngle(Vector3 center, Vector3 currPosition, Vector3 up, float angleDelta)
    {
        //check if mouse is moving
        var lastPosAngleDelta = Vector3.SignedAngle(_lastPosition - center, currPosition - center, up) % 360f;
       // if the angle is smaller then 180+flipDelta instead of rotate -90 rotate +90
       //the reason for this implemntation 
       var clockwise = lastPosAngleDelta >= 0f;

       // return clockwise ? Mathf.Abs(angleDelta) : -Mathf.Abs(angleDelta);
       if (clockwise && angleDelta < -180f + flipDelta || !clockwise && angleDelta > 180f - flipDelta)
        {
            OrigDir = Quaternion.AngleAxis(clockwise ? flipAngle : -flipAngle, up) * OrigDir;
            angleDelta = -angleDelta;
        }

        return angleDelta;
    }

    private Vector3 GetPointOnPlane(Vector3 mousePos,Transform trans)
    {
        Plane _plane = new Plane(Vector3.up, Vector3.zero);
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
    
    // private void RotateWorld()
    // {
    //     foreach (var world in worlds)
    //     {
    //         if (world.GetComponent<World>().isKlydeOn) continue;
    //         Vector3 mousePos = Input.mousePosition;
    //         Vector2 tempPos = mousePos - _mousePosStart;
    //         Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempPos.x, 0, tempPos.y), Vector3.up);
    //         float maxRotationAngle = rotationSpeed;
    //
    //         Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, maxRotationAngle);
    //         world.transform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y, world.transform.rotation.eulerAngles.z);
    //         
    //     }
    //     // Snap to the nearest multiple of snapAngle
    //     if (!GetComponent<AudioSource>().isPlaying)
    //         GetComponent<AudioSource>().Play();
    // }

    private void RotateWorld()
    {
        float deltaX = Input.GetAxis("Mouse X");
        var rotationAmountX = rotationSpeed * deltaX * 0.5f;
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;

            Vector3 mousePos = Input.mousePosition;
            Vector3 tempPos = mousePos - _mousePosStart;

            float rotationSpeed = 10f; // Adjust this value as per your requirement
            
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempPos.x, 0, tempPos.y), Vector3.up);
            Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, rotationSpeed);
            // Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, rotationSpeed * 0.5f);
            world.transform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y,
                world.transform.rotation.eulerAngles.z);
        }
    }
}
