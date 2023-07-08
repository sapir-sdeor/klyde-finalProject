using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindowLevel0 : MonoBehaviour
{
    [SerializeField] private GameObject rightPlane, leftPlane;
    [SerializeField] private AudioSource  _audioWindow;
    private float _soundTime;
    [SerializeField] private float _soundTimeLimit= 1.5f;
    private static bool _level0SoundPlay=true;
    // Start is called before the first frame update
    void Start()
    {
        rightPlane.GetComponent<Animator>().SetBool("recognizeShape",true); 
        leftPlane.GetComponent<Animator>().SetBool("recognizeShape",true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_level0SoundPlay){
            _soundTime += Time.deltaTime;
            if (_soundTime > _soundTimeLimit)
            {
                _level0SoundPlay = false;
                if (!_audioWindow.isPlaying)
                    _audioWindow.Play();
            }
        }
    }
}
