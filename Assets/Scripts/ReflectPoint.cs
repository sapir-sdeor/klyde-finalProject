﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectPoint : MonoBehaviour
{
    [SerializeField] private int halfNum;
    [SerializeField] private float offset;
    private float _angle;
    private GameObject _child;
    private GameObject _mirror;
    private bool enable;
    [SerializeField] private bool thisCollider;
    
    private void Start()
    {
        _child = GetComponentsInChildren<Transform>()[1].gameObject;
        _mirror = GetComponentsInChildren<Transform>()[2].gameObject;
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }

    private void Update()
    {
        var trans = _child.transform;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        if (currAngle < 0) currAngle += 360f;

        // Calculate the adjusted angle range based on the number of halves
        float startAngle = offset + _angle * (halfNum - 1);
        float endAngle = offset + _angle * halfNum;

        // Handle wraparound from 360 to 0 degrees
        if (endAngle > 360f)
            endAngle -= 360f;

        /*if (thisCollider && transform.position.x <= -7.5)
        {
            enable = false;
            EnabledCollider(enable);
        }*/
        // Check if the current angle is within the adjusted range
         if (IsAngleWithinRange(currAngle, startAngle, endAngle))
        {
            enable = true;
            EnabledChild(enable);
        }
        else
        {
            enable = false;
            EnabledChild(enable);
        }
    }

    public bool IsPointSeen()
    {
        return enable;
    }
    
    private bool IsAngleWithinRange(float angle, float start, float end)
    {
        // Normalize the angles to ensure proper comparison
        angle = NormalizeAngle(angle);
        start = NormalizeAngle(start);
        end = NormalizeAngle(end);

        // Check if the angle is within the specified range
        if (start <= end)
        {
            return angle >= start && angle <= end;
        }
        return angle >= start || angle <= end;
    }
    
    private float NormalizeAngle(float angle)
    {
        if (angle < 0f)
        {
            angle %= 360f;
            angle += 360f;
        }
        else if (angle >= 360f)
        {
            angle %= 360f;
        }
        return angle;
    }
    
    public void EnabledChild(bool enabled)
    {
        _child.gameObject.SetActive(enabled);
        _mirror.gameObject.SetActive(enabled);
    }
    
    public void EnabledCollider(bool enabled)
    {
        _child.gameObject.SetActive(enabled);
    }
}
