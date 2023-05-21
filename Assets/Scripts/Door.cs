using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int halfNum;
    private float _angle;
    private List<Transform> _childs = new();

    private bool _doorAppear;
    // Start is called before the first frame update
    private void Start()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child != transform)
            {
                _childs.Add(child);
                child.gameObject.SetActive(false);
            }
        }
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }


    // Update is called once per frame
    void Update()
    {
        if (!RecognizeShape.GetRecognizeShape()) return;
        if (RecognizeShape.GetRecognizeShape() && !_doorAppear)
        {
            foreach (var child in _childs)
            {
                child.gameObject.SetActive(true);
            }
            _doorAppear = true;
        }
        var trans = transform;
        // var currAngle = trans.eulerAngles.y;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.Angle(Vector3.forward, direction);
        if (trans.position.x < 0) currAngle = 360 - currAngle;
        if ( _angle*(halfNum-1) <= currAngle && currAngle <= _angle*(halfNum))
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

    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.gameObject.CompareTag("klyde"))
    //     {
    //         print("klyde win");
    //         LevelManager.NextLevel();
    //     }
    // }
}
