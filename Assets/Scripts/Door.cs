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
             //   child.gameObject.SetActive(false);
                child.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }


    // Update is called once per frame
    void Update()
    {
        if ((RecognizeShape.GetRecognizeShape() && !_doorAppear) || LevelManager.GetLevel() == 0)
        {
            foreach (var child in _childs)
            {
                child.GetComponent<MeshRenderer>().material.color = new Color32(200, 111, 103,255);
            }
            _doorAppear = true;
        }
        var trans = transform;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("klyde") && _doorAppear)
        {
             print("klyde win");
             GetComponent<PlayableDirector>().Play();
             LevelManager.NextLevel();
             other.GetComponent<NavMeshAgent>().enabled = false;
            // other.transform.parent = transform;
             _doorAppear = false;
        }
    }

}
