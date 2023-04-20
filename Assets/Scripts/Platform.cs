using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("platform")) {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            transform.position = collision.contacts[0].point;
        }
    }*/
}
