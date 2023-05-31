using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMap : MonoBehaviour
{
    [SerializeField] private Circle circle;
    // Update is called once per frame
    void Update()
    {
        var rotation = transform.eulerAngles;
        rotation.z = circle.GetRotation();
        transform.eulerAngles = rotation;
    }
}
