using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFixed : MonoBehaviour
{
    private bool isDragging;
    private Vector3 previousMousePosition;

    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private float rotationSpeed = 20f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Calculate the snapped rotation based on the current rotation
            Quaternion currentRotation = transform.rotation;
            Quaternion snappedRotation = Quaternion.identity;

            float xAngle = Mathf.Round(currentRotation.eulerAngles.x / snapAngle) * snapAngle;
            float yAngle = Mathf.Round(currentRotation.eulerAngles.y / snapAngle) * snapAngle;
            float zAngle = Mathf.Round(currentRotation.eulerAngles.z / snapAngle) * snapAngle;

            snappedRotation.eulerAngles = new Vector3(xAngle, yAngle, zAngle);
            transform.rotation = snappedRotation;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            transform.Rotate(Vector3.up, -mouseDelta.x * Time.deltaTime * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseDelta.y * Time.deltaTime * rotationSpeed, Space.World);
            previousMousePosition = Input.mousePosition;
        }
    }
}
