using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    
    [SerializeField] private float anglesRotate = 35;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera cam;
    private float rotationTime = 0.2f;
    private float _time;
    private float _xRotation;
    private float _yRotation;
    private float _zRotation;
    private float _xCurrRotation;
    private float _yCurrRotation;
    private float _zCurrRotation;
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _time > rotationTime)
        {
            _xRotation = Input.GetAxis("Mouse Y");
            _yRotation = Input.GetAxis("Mouse X");
            _zRotation = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(_xRotation) > Mathf.Abs(_yRotation) && Mathf.Abs(_xRotation) > Mathf.Abs(_zRotation)) {
                // Rotate around x-axis
                _xCurrRotation += Mathf.Sign(_xRotation) * anglesRotate;
            } else if (Mathf.Abs(_yRotation) > Mathf.Abs(_xRotation) && Mathf.Abs(_yRotation) > Mathf.Abs(_zRotation)) {
                // Rotate around y-axis
                _yCurrRotation += Mathf.Sign(_yRotation) * anglesRotate;
            } else if (Mathf.Abs(_zRotation) > Mathf.Abs(_xRotation) && Mathf.Abs(_zRotation) > Mathf.Abs(_yRotation)) {
                // Rotate around z-axis
                _zCurrRotation += Mathf.Sign(_zRotation) * anglesRotate;
            }
            transform.rotation = Quaternion.Euler(_xCurrRotation, _yCurrRotation, _zCurrRotation);
            _time = 0;
        }
        _time += Time.deltaTime;
        
    }

    private void OnMouseDrag()
    {
        //TODO: need to do different for touch?
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
        Vector3 right = Vector3.Cross(cam.transform.up, transform.position - cam.transform.position);
        Vector3 up = Vector3.Cross(transform.position - cam.transform.position, right);
        transform.rotation = Quaternion.AngleAxis(-rotX, up) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(rotY, right) * transform.rotation;
    }
    
    
    /*foreach (var touch in Input.touches)
       {
           Ray camRay = cam.ScreenPointToRay(touch.position);
           RaycastHit raycastHit;
           if (Physics.Raycast(camRay, out raycastHit, 10))
           {
               if (touch.phase == TouchPhase.Moved)
               {
                   transform.Rotate(touch.deltaPosition.y * rotTouchSpeed,
                       -touch.deltaPosition.x * rotTouchSpeed, 0 , Space.World);
               }
           }
       }*/
}
