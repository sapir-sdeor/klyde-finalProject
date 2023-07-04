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
    [SerializeField] private float timeForRotateMusic = 0.3f;
    [SerializeField] private Vector3[] rotationsRecognizeShape;
    [SerializeField] private float timeToWaitBetweenRotation = 0.05f;
    private float _millisecCounter;
    private float _timeCounter;
    private float _currentTime;
    private bool _isWalking;
    private static bool _stopRotate;
    private static bool _isRotatingMore;
    [SerializeField]
    private float speed = 150f;
    

    private Vector3 _lastPosition;
    private GameObject _lastObj;
    private static bool _automaticCompleteTheShape;

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
            _millisecCounter = Time.time;
            _timeCounter = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            BuildNewNavMesh();
            _isRotatingMore = false;
            GetComponent<AudioSource>().Stop();
        }
        else if (Input.GetMouseButton(0))
        {
            RemoveAllNavMesh();
        }
        
        if (_isDragging && !moving.GetIsWalk() && !_stopRotate )
        {
            PerformCircularRotation();
        }

        if (_automaticCompleteTheShape)
        {
            AutomaticCompleteTheShapeHelper();
        }
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
            _isRotatingMore = true;
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }
    }

    private void BuildNewNavMesh()
    {
        _isRotating = false;
        _isDragging = false;
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces[i].BuildNavMesh();
        }

    }
    
    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
    public static bool GetIsRotatingMore()
    {
        return _isRotatingMore;
    }
    
    
    private void PerformCircularRotation()
    {
        print(worlds.Length + "world length");
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
              print(world.name + " world name angle delta "+angleDelta);

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
    
    public static void AutomaticCompleteTheShape()
    {
        _automaticCompleteTheShape = true;
    }
    
    private void AutomaticCompleteTheShapeHelper()
    {
        _automaticCompleteTheShape = false;
        StartCoroutine(RotateShapes());
    }

    private IEnumerator RotateShapes()
    {
        _stopRotate = true;
        moving.SetStopWalk(true);
        int index = 0;
        foreach (var world in worlds)
        {
            Vector3 currentRotation = world.transform.eulerAngles;
            Vector3 targetRotation = rotationsRecognizeShape[index];
            float rotationSpeed = speed/10; // Adjust the rotation speed as needed

            while (Quaternion.Euler(currentRotation) != Quaternion.Euler(targetRotation))
            {
                Quaternion fromRotation = Quaternion.Euler(currentRotation);
                Quaternion toRotation = Quaternion.Euler(targetRotation);
                float rotationStep = rotationSpeed * Time.deltaTime;
                world.transform.rotation = Quaternion.RotateTowards(fromRotation, toRotation, rotationStep);
                currentRotation = world.transform.eulerAngles;
                yield return null;
            }
            // Delay the start of the second rotation
            yield return new WaitForSeconds(timeToWaitBetweenRotation); // Adjust the delay time as needed
            index++;
        }
        RecognizeShape.SetShowObject();
        _stopRotate = false;
        moving.SetStopWalk(false);
    }

    public static void SetStopRotate(bool val)
    {
        _stopRotate = val;
    }
    
    
}
