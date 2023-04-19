using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class rotate2exis : MonoBehaviour
{
    [SerializeField] private List<Transform> axis1;
    [SerializeField] private List<Transform> axis2;
    [SerializeField] private NavMeshSurface[] surfaces;

    private bool _isRotating = false;
    private Vector3 _previousMousePos;
    void Start()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }  
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            _isRotating = true;
            _previousMousePos = Input.mousePosition;
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            for (int i = 0; i < surfaces.Length; i++) 
            {
                surfaces [i].BuildNavMesh ();    
            }    
        }

        // Rotate the object if the left mouse button is pressed down
        if (_isRotating)
        {
            // Get the current mouse position
            Vector3 currentMousePos = Input.mousePosition;

            // Calculate the distance between the current and previous mouse positions
            float deltaX = Mathf.Abs(currentMousePos.x - _previousMousePos.x);
            float deltaY = Mathf.Abs(currentMousePos.y - _previousMousePos.y);

            // Rotate the object based on the mouse movement
            if (deltaX > deltaY)
            {
                float rotationAmount = (currentMousePos.x - _previousMousePos.x) * 0.1f;
                foreach (var obj in axis2)
                {
                    obj.Rotate(Vector3.up, rotationAmount, Space.World);
                }
            }
            else if (deltaY > deltaX)
            {
                float xRotationAmount = -(currentMousePos.y - _previousMousePos.y) * 0.1f;
                float zRotationAmount = (currentMousePos.x - _previousMousePos.x) * 0.1f;
                foreach (var obj in axis1)
                {
                    obj.Rotate(new Vector3(xRotationAmount, 0, zRotationAmount), Space.World);
                }

            }

            // Store the current mouse position as the previous mouse position for the next frame
            _previousMousePos = currentMousePos;
        }
    }
}
