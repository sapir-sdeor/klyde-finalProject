using System;
using UnityEngine;

public class RotateZViaDrag : MonoBehaviour
{
    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    [Range(0, 360)]
    private int clickAngle = 5;

    [SerializeField]
    private float stayAtClickTime = 0f;

    [Space]
    [SerializeField]
    private bool createObj = true;

    [SerializeField]
    private GameObject obj;

    private GameObject _lastObj;
    private Plane _plane;
    private bool _isMouseUp;
    private bool _finishedAdjust;
    private Vector3 _lastPosition;
    private Vector3 _lastTarget;
    private float _waitTimer;

    private Vector3 OrigDir { get; set; }

    private Transform MyTransform { get; set; }

    private void Awake()
    {
        MyTransform = transform;
        _plane = new Plane(MyTransform.up, MyTransform.position);
        _isMouseUp = true;
        _finishedAdjust = true;
    }

    private void OnMouseDown()
    {
        _lastPosition = GetPointOnPlane(Input.mousePosition);
        _lastTarget = GetClosestClick(_lastPosition - MyTransform.position);
        OrigDir = _lastTarget;
        if (createObj)
        {
            obj.SetActive(true);
            obj.transform.position = _lastPosition;
        }

        _isMouseUp = false;
        _finishedAdjust = true;
    }

    private void OnMouseUp()
    {
        if (createObj)
        {
            obj.SetActive(false);
        }

        _isMouseUp = true;
        _finishedAdjust = false;
    }

    private void Update()
    {
        if (_waitTimer < stayAtClickTime)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer < stayAtClickTime)
            {
                return;
            }
        }   
        
        if (!_isMouseUp || !_finishedAdjust)
        {
            PerformCircularRotation();
        }
    }

    private void FixedUpdate()
    {
        var forward = MyTransform.right;
        var center = MyTransform.position;
        
        for (int i = 0; i < 360; i += clickAngle)
        {
            var angleAxis = Quaternion.AngleAxis(i, MyTransform.up);
            // Debug.DrawRay(center, angleAxis * forward, color, Time.deltaTime);
            var dir = angleAxis * Vector3.right;
            Debug.DrawRay(center, dir , Color.gray, Time.deltaTime);
        }

        Debug.DrawRay(center, forward, new Color(0.32f, 0.33f, 0.5f), Time.deltaTime);
        Debug.DrawRay(center, Vector3.right, Color.black, Time.deltaTime);
        Debug.DrawRay(center, OrigDir, Color.red, Time.deltaTime);
        Debug.DrawRay(center, _lastTarget, Color.green, Time.deltaTime);
    }
    

    private void PerformCircularRotation()
    {
        var adjustedSpeed = Time.deltaTime * speed;

        var center = MyTransform.position;
        var up = MyTransform.up;
        var rotation = MyTransform.rotation;

        // _lastPosition = GetPointOnPlane(Input.mousePosition);
        var curTargetDir = _lastTarget;

        var angleDelta = Vector3.SignedAngle(OrigDir, curTargetDir, up) % 360f;
        angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);
        
        Debug.DrawLine(center, _lastPosition, Color.yellow);
        
        // rotate by that much
        var rot = Quaternion.AngleAxis(angleDelta, up);
        MyTransform.rotation = rotation * rot;
        if (Mathf.Abs(angleDelta) < 0.1f)
        {
            _finishedAdjust = true;
            OrigDir = _lastTarget;
            _lastPosition = GetPointOnPlane(Input.mousePosition);
            _lastTarget = GetClosestClick(_lastPosition - center);
            angleDelta = Vector3.SignedAngle(OrigDir, _lastTarget, up) % 360f;
            angleDelta = Mathf.Clamp(angleDelta, -clickAngle, clickAngle);
            OrigDir = Quaternion.AngleAxis(-angleDelta, up) * _lastTarget;
            _waitTimer = 0f;
        }
        else
        {
            _finishedAdjust = false;
            OrigDir = rot * OrigDir;
        }
    }
    

    private Vector3 GetPointOnPlane(Vector3 mousePos)
    {
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }

    private Vector3 GetClosestClick(Vector3 curDir)
    {
        var globalRight = Vector3.right;
        var up = MyTransform.up;

        var curAngle = Vector3.SignedAngle(curDir, globalRight, up) % 360f;
        var closestFiveAngle = (int) (clickAngle * Mathf.Floor(curAngle / clickAngle)) % 360;
        var closestFiveDeg = Quaternion.AngleAxis(-closestFiveAngle, up) * globalRight;
        return closestFiveDeg;
    }

}
