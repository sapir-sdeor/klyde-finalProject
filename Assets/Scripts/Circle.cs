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

        isKlydeOn =(_angle * (halfNum - 1)  <= currAngle && currAngle <= _angle * halfNum) ;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (!child.gameObject.CompareTag("circle")) continue;
            if (RecognizeShape.GetRecognizeShape() && !_changeTexture)
            {
                child.GetComponent<MeshRenderer>().material.mainTexture = textureWithoutShape;
                _changeTexture = true;
            }
            print(_angle + " " + halfNum);
            child.GetComponent<MeshRenderer>().material.SetFloat("_Angle", _angle);
            child.GetComponent<MeshRenderer>().material.SetInt("_HalfNum", halfNum);
        }
    }
}
  
