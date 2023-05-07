using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateKaleidoscope : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f;

    // private Material material;

    void Start()
    {
        // Renderer renderer = GetComponent<Renderer>();
        // material = renderer.material;
    }
    
    void Update()
    {
        // Calculate the new angle based on the current time and rotation speed

        // Set the angle property in the shader
        // material.SetFloat("_Angle", angle);
        float angle = Time.time * rotationSpeed; // calculate the new angle based on input or other factors
        Vector2 movement = Vector2.one;// calculate the new movement vector based on input or other factors
        GetComponent<Renderer>().material.SetFloat("_KaleidoscopeAngle", angle);
        GetComponent<Renderer>().material.SetVector("_KaleidoscopeMovement", movement);
        
        
    }
}

