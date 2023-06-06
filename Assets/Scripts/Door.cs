using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Door : MonoBehaviour
{
    [SerializeField] private int halfNum;
    [SerializeField] private float offset;
    [SerializeField] private float speed = 10f;
    private float _angle;
    private List<Transform> _childs = new();
    private bool _win, _rotateOnce,_wrong,_getBack, _doorAppear,_touchBallon;
    [SerializeField] private Vector3 TargetInit = new Vector3(0.300000012f, 6.30000019f, -18f);
    private Vector3 _target;
    private Vector3 _firstPos;
    private GameObject _klyde;
    // Start is called before the first frame update
    private void Start()
    {
        _target = TargetInit;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child != transform)
            {
                _childs.Add(child);
             //   child.gameObject.SetActive(false);
                child.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }


    // Update is called once per frame
    // void Update()
    // {
    //     if ((RecognizeShape.GetRecognizeShape() && !_doorAppear) || LevelManager.GetLevel() == 0)
    //     {
    //         foreach (var child in _childs)
    //         {
    //             child.GetComponent<MeshRenderer>().material.color = new Color32(200, 111, 103,255);
    //         }
    //         _doorAppear = true;
    //     }
    //     var trans = transform;
    //     Vector3 direction = trans.position - Vector3.zero;
    //     // Calculate the angle between the direction vector and the forward vector
    //     float currAngle = Vector3.Angle(Vector3.forward, direction);
    //     if (trans.position.x < 0) currAngle = 360 - currAngle;
    //     if ( _angle*(halfNum-1) <= currAngle && currAngle <= _angle*(halfNum))
    //     {
    //         EnabledDoorChild(true);
    //     }
    //     else
    //     {
    //        EnabledDoorChild(false);
    //     }
    // }
    //
    void Update()
    {
        if (_win)
            MoveToVitrajWinCase();
        
        else if (_wrong && !_touchBallon)
            MoveToVitrajWrongCase();
        
        if ((RecognizeShape.GetRecognizeShape() && !_doorAppear) || LevelManager.GetLevel() == 0)
        {
            foreach (var child in _childs)
            {
                child.GetComponent<MeshRenderer>().material.color = new Color32(200, 111, 103, 255);
            }
            _doorAppear = true;
        }

        var trans = transform;
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

        // Check if the current angle is within the adjusted range
        if (IsAngleWithinRange(currAngle, startAngle, endAngle))
        {
            EnabledDoorChild(true);
        }
        else
        {
            EnabledDoorChild(false);
        }
    }


    private void EnabledDoorChild(bool enabled)
    {
        foreach (var child in GetComponentsInChildren<MeshRenderer>())
        {
            child.GetComponent<MeshRenderer>().enabled = enabled;
        }
        GetComponent<Collider>().enabled = enabled;
    }
    //todo: understand how collider work with ballon

    private void OnTriggerEnter(Collider other)
    {
        print("_doorAppear "+_doorAppear);
        if(other.gameObject.CompareTag("klyde") && _doorAppear)
        {
             print("klyde win");
           //  GetComponent<PlayableDirector>().Play();
             _klyde = other.gameObject;
             _win = true;
             _klyde.transform.eulerAngles =
                 new Vector3(_klyde.transform.eulerAngles.x, 180, _klyde.transform.eulerAngles.z);
             _klyde.GetComponent<moving>().SetWalkAnimationFalse();
             other.GetComponent<NavMeshAgent>().enabled = false;
            // other.transform.parent = transform;
             _doorAppear = false;
             _target = TargetInit;
        }
        else if (other.gameObject.CompareTag("klyde") && !_wrong)
        {
            _wrong = true;
            _firstPos = other.transform.position;
            _klyde = other.gameObject;
            _klyde.transform.eulerAngles =
                new Vector3(_klyde.transform.eulerAngles.x, 180, _klyde.transform.eulerAngles.z);
            _target = TargetInit;
        }
        
    }
    
    // Helper method to check if an angle is within a specified range
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
        else
        {
            return angle >= start || angle <= end;
        }
    }

// Helper method to normalize an angle to the range of 0-360 degrees
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


    private void MoveToVitrajWinCase()
    {
        print("win");
        _klyde.GetComponent<NavMeshAgent>().enabled = false;
        _klyde.GetComponent<moving>().enabled = false;
        _klyde.transform.position = Vector3.MoveTowards(_klyde.transform.position, _target,
            Time.deltaTime * speed);
        if (!_rotateOnce)
        {
            _klyde.transform.Rotate(45, 0, 0);
            _rotateOnce = true;
        }
        
        if (Vector3.Distance(_klyde.transform.position, _target) < 0.01f)
        {
            LevelManager.NextLevel();
            _win = false;
        }
    }

    private void MoveToVitrajWrongCase()
    {
        _klyde.GetComponent<NavMeshAgent>().enabled = false;
        _klyde.GetComponent<moving>().enabled = false;
        _klyde.transform.position = Vector3.MoveTowards(_klyde.transform.position, _target,
            Time.deltaTime * 5);
        if (Vector3.Distance(_klyde.transform.position, _target) < 0.01f && !_getBack)
        {
            _getBack = true;
            _target = _firstPos;
        }
        else if (Vector3.Distance(_klyde.transform.position, _target) < 0.01f)
        {
            _wrong = false;
            _klyde.transform.eulerAngles =
                new Vector3(_klyde.transform.eulerAngles.x, 0, _klyde.transform.eulerAngles.z);
            _klyde.GetComponent<NavMeshAgent>().enabled = true;
            _klyde.GetComponent<moving>().enabled = true;
            _touchBallon = true;
        }
    }
}
