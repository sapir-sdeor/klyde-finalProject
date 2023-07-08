using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ParticalsPath : MonoBehaviour
{
    [SerializeField] private GameObject klea;
    [SerializeField] private float timeToPoint = 1f;
    private Vector3[] _points = new[]
    {
        Vector3.zero,
        new Vector3(-8.5f,9f,0f),
        new Vector3(3.2f,13.5f,-2.9f),
        new Vector3(-3.5f,32.7f,25) //shine position
    };
    private int _pointIndex;
    private Vector3 targetPos;
    private ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_pointIndex - 1 == _points.Length)
        {
            particleSystem.Pause();
            return;
        }
        targetPos = _pointIndex == 0 ? klea.transform.position : _points[_pointIndex - 1];
        transform.position = Vector3.MoveTowards(transform.position, targetPos, timeToPoint * Time.deltaTime);
        if (transform.position == targetPos)
        {
            _pointIndex++;
        }
    }
    
    
    /*
    private void LateUpdate()
    {
        int particleCount = particleSystem.GetParticles(particles);

        if (particleCount > initialPositions.Length)
        {
            initialPositions = new Vector3[particleCount];
        }

        for (int i = 0; i < particleCount; i++)
        {
            if (particles[i].remainingLifetime == particles[i].startLifetime)
            {
                initialPositions[i] = particles[i].position; // Store initial position

                particles[i].velocity = Vector3.zero; // Set velocity to zero to freeze the particle
                particles[i].remainingLifetime = Mathf.Epsilon; // Set a small remaining lifetime to prevent immediate removal
            }
            else
            {
                particles[i].position = initialPositions[i]; // Set position to initial position
            }
        }
        particleSystem.SetParticles(particles, particleCount);
    }*/
    
}