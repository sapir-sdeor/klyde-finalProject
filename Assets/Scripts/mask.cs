using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mask : MonoBehaviour
{
    [SerializeField] private int renderQueue = 3002;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = renderQueue;
    }

    // Update is called once per frame
  
}
