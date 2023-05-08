using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle1 : World
{
    private GameObject klyde;
    // Start is called before the first frame update
    void Start()
    {
        //todo: make it general
        klyde = GameObject.FindGameObjectWithTag("klyde");
    }

    // Update is called once per frame
    void Update()
    {
        isKlydeOn = klyde.transform.position.x < 0f;
    }
}
