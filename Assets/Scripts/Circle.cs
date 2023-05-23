using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Circle : World
{
    private GameObject klyde;
    private float _angle;
    private bool _changeTexture;
    [SerializeField] private int halfNum;
    
    void Start()
    {
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
        klyde = GameObject.FindGameObjectWithTag("klyde");
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.CompareTag("circle")) continue;
            print(_angle + " " + halfNum);
            child.GetComponent<MeshRenderer>().material.SetFloat("_Angle", _angle);
            child.GetComponent<MeshRenderer>().material.SetInt("_HalfNum", halfNum);
        }
       
    }
    
    // Update is called once per frame
    void Update()
    {
        var trans = klyde.transform;
        // var currAngle = trans.eulerAngles.y;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.Angle(Vector3.forward, direction);
        if (trans.position.x < 0) currAngle = 360 - currAngle;
        float eularRotation = transform.localEulerAngles.y;
        print(eularRotation + " circle Angle " + halfNum);

        isKlydeOn =(_angle * (halfNum - 1)  <= currAngle && currAngle <= _angle * halfNum) ;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.CompareTag("circle")) continue;
            float childAngle = child.transform.localEulerAngles.x;
            childAngle = eularRotation;
            if (childAngle < 0) childAngle = 360 + childAngle;
            if (childAngle > 360) childAngle = childAngle - 360;
            print(childAngle + " childAngle " + halfNum);
            
            child.GetComponent<MeshRenderer>().material.SetFloat("_CurrAngle", childAngle);
            // float _klydeFloatBool = isKlydeOn ? 1 : 0;
            // child.GetComponent<MeshRenderer>().material.SetFloat("_Klyde", _klydeFloatBool);
        }
        
    }
}
  
