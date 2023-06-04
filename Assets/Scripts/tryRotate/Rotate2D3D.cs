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
    
    [SerializeField] private NavMeshSurface[] surfaces;
    [SerializeField] private float SeprateBetweenRotateWalk;
    [SerializeField] private float maxRotationAmount = 10f;
    private float millisecCounter;
    private float currentTime;
    private bool isWalking;
    
    void Start()
    {
        BuildNewNavMesh();
        print("build navmesh");
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
            /*Vector3 localPos = new Vector3(worlds[0].transform.parent.transform.position.x, 0,
                worlds[0].transform.parent.transform.position.z);
            _pos = _mousePosStart - localPos;*/
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
        }
    }

    private void RemoveAllNavMesh()
    {
        // RotateWorld();
        // print(Time.time - millisecCounter + " button down");
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

    private void RotateWorld()
    {
        /*float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");
       
        deltaX *= sensitivity;
        deltaY *= sensitivity;*/
        
        /*var rotationAmountX = rotationSpeed * deltaX * 0.5f;
        var rotationAmountY = rotationSpeed * deltaY * 0.5f;

        rotationAmountX = Mathf.Clamp(rotationAmountX, -maxRotationAmount, maxRotationAmount);
        rotationAmountY = Mathf.Clamp(rotationAmountY, -maxRotationAmount, maxRotationAmount);*/

        // Rotate the worlds around the pivot point based on mouse movement
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;
            
            /*
            Vector2 mousePos = Input.mousePosition;
            Vector2 tempPos = mousePos - _mousePosStart;
            
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempPos.x, 0, tempPos.y), Vector3.up);
            Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, rotationSpeed);
            world.transform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y, 
                world.transform.rotation.eulerAngles.z);
                */
            /*Vector3 mousePos = GetPlayerPlaneMousePos(Input.mousePosition);
            Vector3 tempPos = mousePos - _mousePosStart;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tempPos.x, 0, tempPos.y), Vector3.up);
            float maxRotationAngle = rotationSpeed;

            Quaternion rotation = Quaternion.RotateTowards(world.transform.rotation, targetRotation, maxRotationAngle);
            world.transform.rotation = Quaternion.Euler(world.transform.rotation.eulerAngles.x, rotation.eulerAngles.y, world.transform.rotation.eulerAngles.z);*/
            // Calculate the difference in mouse position
            Vector3 mouseDelta = Input.mousePosition - _mousePosStart;

            float angle = Mathf.Atan2(mouseDelta.x, mouseDelta.y) * Mathf.Rad2Deg;

            world.transform.rotation = Quaternion.Euler(world.transform.eulerAngles.x, -angle, world.transform.eulerAngles.z);

            _mousePosStart = Input.mousePosition;
            /*var position = pivotPoint.position;
            if (ang > 0)
            {
                world.transform.RotateAround(position, Vector3.up, rotationAmountX);
                world.transform.RotateAround(position, Vector3.up, rotationAmountY);
            }
            else
            {
                world.transform.RotateAround(position, Vector3.up, -rotationAmountX);
                world.transform.RotateAround(position, Vector3.up, -rotationAmountY);
            }*/
            /*var currentRotation = world.transform.rotation;
            var xAngle = Mathf.Round(currentRotation.eulerAngles.x / snapAngle) * snapAngle;
            var yAngle = Mathf.Round(currentRotation.eulerAngles.y / snapAngle) * snapAngle;
            var zAngle = Mathf.Round(currentRotation.eulerAngles.z / snapAngle) * snapAngle;
            world.transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);*/
        }
        // Snap to the nearest multiple of snapAngle
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
    }

    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
    public Vector3 GetPlayerPlaneMousePos(Vector3 aPlayerPos)
    {
        Plane plane = new Plane(Vector3.up, aPlayerPos);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            return ray.GetPoint(dist);
        }
        return Vector3.zero;
    }
    
}
