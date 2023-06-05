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
            _mousePosStart = GetPointOnPlane(Input.mousePosition);
            Vector3 localPos = new Vector3(worlds[0].transform.parent.transform.position.x, 0,
                worlds[0].transform.parent.transform.position.z);
            _pos = _mousePosStart - localPos;
            _lastPosition = GetPointOnPlane(Input.mousePosition);
            // OrigDir = _lastPosition - MyTransform.position;
            OrigDir = _lastPosition-worlds[1].transform.parent.transform.position;
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
            RotateWorld();
            // PerformCircularRotation();
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
            var center = worldTransform.position;
            // Vector3 center = new Vector3(worlds[0].transform.parent.transform.position.x, 0,
            //     worlds[0].transform.parent.transform.position.z);
            // var center = Vector3.zero;
            var up = worldTransform.up;
            var rotation = worldTransform.rotation;

            var currPosition = GetPointOnPlane(Input.mousePosition);
            print("currPosition "+ currPosition);

            var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) % 360f;
            // angleDelta = HandleFlipAngle(center, currPosition, up, angleDelta);
            // TODO: Any smoothing will probably go here
            // angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);

            _lastPosition = currPosition;

            Debug.DrawLine(center, currPosition, Color.green);
            Debug.DrawRay(center, OrigDir, Color.red);

            // rotate by that much
            var rot = Quaternion.AngleAxis(angleDelta, up);
            OrigDir = rot * OrigDir;
            rotation*= rot;
            worldTransform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y, world.transform.rotation.eulerAngles.z);;
        }
        
    } 
    
    private float HandleFlipAngle(Vector3 center, Vector3 currPosition, Vector3 up, float angleDelta)
    {
        var lastPosAngleDelta = Vector3.SignedAngle(_lastPosition - center, currPosition - center, up) % 360f;
        var clockwise = lastPosAngleDelta >= 0f;
        if (clockwise && angleDelta < -180f + flipDelta || !clockwise && angleDelta > 180f - flipDelta)
        {
            OrigDir = Quaternion.AngleAxis(clockwise ? flipAngle : -flipAngle, up) * OrigDir;
            if (createObj)
            {
                obj.transform.position = center + OrigDir;
            }

            angleDelta = -angleDelta;
        }

        return angleDelta;
    }

    private Vector3 GetPointOnPlane(Vector3 mousePos)
    {
        Plane _plane = new Plane(Vector3.up, mousePos);
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }
    
    private void RotateWorld()
    {
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;
            Vector3 mousePos = GetPointOnPlane(Input.mousePosition);
            Vector3 tempPos = mousePos - _mousePosStart;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempPos.x, 0, tempPos.y), Vector3.up);
            float maxRotationAngle = rotationSpeed;

            Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, maxRotationAngle);
            world.transform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y, world.transform.rotation.eulerAngles.z);
            
        }
        // Snap to the nearest multiple of snapAngle
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }

    

    
}
