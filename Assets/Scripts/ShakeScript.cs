using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeScript : MonoBehaviour
{
    [SerializeField] private float amplitudeGain=1.5f, frequencyGain=1.5f;
    [SerializeField] private float shakeDuration=1;
    private CinemachineVirtualCamera _vcam;
    private float _startTimeShake;

    private CinemachineBasicMultiChannelPerlin noisePerlin;
    // Start is called before the first frame update
    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Shake()
    {
        noisePerlin.m_AmplitudeGain = amplitudeGain;
        noisePerlin.m_FrequencyGain = frequencyGain;
        // if (Time.time - _startTimeShake > shakeDuration)
        // {
        //     Door.SetShake();
        //     n
        // }
    }

    private void CancelShake()
    {
        noisePerlin.m_AmplitudeGain = 0;
        noisePerlin.m_FrequencyGain = 0;
        print("cancel shake");
    }

    // Update is called once per frame
    void Update()
    {
        if (Door.GetShake())
        {
            _startTimeShake = Time.time;
            Shake();
        }
        else CancelShake();
    }
}
