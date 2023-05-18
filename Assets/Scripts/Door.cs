using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int halfNum;
    private float _angle;
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }


    // Update is called once per frame
    void Update()
    {
        if (RecognizeShape.GetRecognizeShape()) gameObject.SetActive(true);
        var trans = transform;
        var currAngle = trans.eulerAngles.y;
        var parentAngle = trans.parent.eulerAngles.z;
        currAngle += parentAngle;
   //     Debug.Log(  " angle: " + currAngle);
        // if(i== transform.childCount-1) Debug.Log("Object " + i + " angle: " + currAngle);
        /*if ( _angle*(halfNum-1) <= currAngle && currAngle <= _angle*(halfNum))
        {
            trans.gameObject.SetActive(true);
        }*/
        /*else
        {
            trans.gameObject.SetActive(false);      
        }*/
        if ( _angle*(halfNum-1) <= currAngle && currAngle <= _angle*(halfNum))
        {
            foreach (var child in GetComponentsInChildren<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            foreach (var child in GetComponentsInChildren<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("klyde"))
        {
            print("klyde win");
            LevelManager.NextLevel();
        }
    }
}
