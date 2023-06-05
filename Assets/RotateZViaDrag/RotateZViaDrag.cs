using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by @kurtdekker

public class RotateZViaDragBar : MonoBehaviour
{
    public bool RotateAroundAxis;

    Vector3 lastPosition;

    void OnMouseDown()
    {
        lastPosition = Input.mousePosition;
    }

    void PerformLinearRotation()
    {
        Vector3 currPosition = Input.mousePosition;

        Vector3 difference = currPosition - lastPosition;

        lastPosition = currPosition;

        // now choose what axis to care about... this adds X and Y
        float change = difference.x + difference.y;

        // and it rotates it around the Z (forward)
        transform.Rotate(new Vector3(0, 0, change));
    }

    void PerformCircularRotation()
    {
		// where is our center on screen?
        Vector3 center = Camera.main.WorldToScreenPoint(transform.position);

		// angle to previous finger
        float anglePrevious = Mathf.Atan2(center.x - lastPosition.x, lastPosition.y - center.y);

        Vector3 currPosition = Input.mousePosition;

		// angle to current finger
        float angleNow = Mathf.Atan2(center.x - currPosition.x, currPosition.y - center.y);

        lastPosition = currPosition;

		// how different are those angles?
        float angleDelta = angleNow - anglePrevious;

		// rotate by that much
        transform.Rotate(new Vector3(0, 0, angleDelta * Mathf.Rad2Deg));
    }

    void OnMouseDrag()
    {
        if (RotateAroundAxis)
        {
            PerformCircularRotation();
        }
		else
        {
            PerformLinearRotation();
        }
    }
}
