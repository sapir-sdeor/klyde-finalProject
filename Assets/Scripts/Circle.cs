using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Circle : World
{
    private GameObject klyde;
    
    
    void Start()
    {
        //todo: make it general
        klyde = GameObject.FindGameObjectWithTag("klyde");
       
    }
    
    // Update is called once per frame
    void Update()
    {
        isKlydeOn = klyde.transform.position.x > 0f;
    }
}
  
