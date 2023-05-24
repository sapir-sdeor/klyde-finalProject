using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    private bool _changeTexture = false;
    [SerializeField] private Texture textureWithoutShape;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (RecognizeShape.GetRecognizeShape() && !_changeTexture)
        {
            GetComponent<MeshRenderer>().material.mainTexture = textureWithoutShape;
            _changeTexture = true;
        }
        
    }
}
