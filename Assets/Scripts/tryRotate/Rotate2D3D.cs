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
    [SerializeField] private float rotationSpeed = 2f;
    private static bool _isRotating ;
    private static bool _isDragging ;
    public Transform pivotPoint;
    private Vector2 mouseDirection;
    
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
        print("out " + (Time.time - millisecCounter));
    }

    private void RemoveAllNavMesh()
    {
        // RotateWorld();
        print(Time.time - millisecCounter + " button down");
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
        // Get the amount of mouse movement since the last frame
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        
        // Multiply by sensitivity to get desired movement
        deltaX *= sensitivity;
        deltaY *= sensitivity;

        /*
        deltaX = Mathf.Round(deltaX / snapAngle) * snapAngle;
        deltaY = Mathf.Round(deltaY / snapAngle) * snapAngle;*/
        var rotationAmountX = rotationSpeed * deltaX * 0.5f;
        var rotationAmountY = rotationSpeed * deltaY * 0.5f;
      //  float mouseMovementMagnitude = new Vector2(deltaX, deltaY).magnitude;
      //  float mouseSpeedFactor = Mathf.Clamp01(mouseMovementMagnitude / maxRotationAmount);
      /*
      rotationAmountX *= mouseSpeedFactor;
      rotationAmountY *= mouseSpeedFactor;
      */
     
        rotationAmountX = Mathf.Clamp(rotationAmountX, -maxRotationAmount, maxRotationAmount);
        rotationAmountY = Mathf.Clamp(rotationAmountY, -maxRotationAmount, maxRotationAmount);

        // Rotate the worlds around the pivot point based on mouse movement
        foreach (var world in worlds)
        {
            if (world.GetComponent<World>().isKlydeOn) continue;
            var position = pivotPoint.position;
            world.transform.RotateAround(position, Vector3.up, rotationAmountX);
            world.transform.RotateAround(position, Vector3.up, rotationAmountY);
            var currentRotation = world.transform.rotation;
            var xAngle = Mathf.Round(currentRotation.eulerAngles.x / snapAngle) * snapAngle;
            var yAngle = Mathf.Round(currentRotation.eulerAngles.y / snapAngle) * snapAngle;
            var zAngle = Mathf.Round(currentRotation.eulerAngles.z / snapAngle) * snapAngle;
            world.transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
        }
        // Snap to the nearest multiple of snapAngle
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
        
    }

    public static bool GetIsRotating()
    {
        return _isRotating;
    }
    
}
