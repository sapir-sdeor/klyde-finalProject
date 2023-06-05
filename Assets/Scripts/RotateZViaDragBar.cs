using UnityEngine;

public class RotateZViaDrag : MonoBehaviour
{
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
        MyTransform = transform;
        _plane = new Plane(MyTransform.up, MyTransform.position);

    }

    private void OnMouseDown()
    {
        _lastPosition = GetPointOnPlane(Input.mousePosition);
        OrigDir = _lastPosition - MyTransform.position;
        if (createObj)
        {
            obj.SetActive(true);
            obj.transform.position = _lastPosition;
        }
    }

    private void OnMouseUp()
    {
        if (createObj)
        {
            obj.SetActive(false);
        }
    }

    private void OnMouseDrag()
    {
        PerformCircularRotation();
    }

    private void PerformCircularRotation()
    {
        var adjustedSpeed = Time.deltaTime * speed;

        var center = MyTransform.position;
        var up = MyTransform.up;
        var rotation = MyTransform.rotation;

        var currPosition = GetPointOnPlane(Input.mousePosition);

        var angleDelta = Vector3.SignedAngle(OrigDir, currPosition - center, up) % 360f;
        angleDelta = HandleFlipAngle(center, currPosition, up, angleDelta);
        // TODO: Any smoothing will probably go here
        angleDelta = Mathf.Clamp(angleDelta, -adjustedSpeed, adjustedSpeed);

        _lastPosition = currPosition;

        Debug.DrawLine(center, currPosition, Color.green);
        Debug.DrawRay(center, OrigDir, Color.red);

        // rotate by that much
        var rot = Quaternion.AngleAxis(angleDelta, up);
        OrigDir = rot * OrigDir;
        MyTransform.rotation = rotation * rot;
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
        var ray = Camera.main!.ScreenPointToRay(mousePos);
        return _plane.Raycast(ray, out var dist) ? ray.GetPoint(dist) : Vector3.zero;
    }

}
