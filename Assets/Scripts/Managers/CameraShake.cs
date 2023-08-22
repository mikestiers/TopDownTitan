using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Duration of the shake in seconds
    public float shakeMagnitude = 0.7f; // Amplitude of the shake. Larger value shakes harder.
    public float dampingSpeed = 1.0f; // Damping speed to return to the initial position

    Vector3 initialPosition;
    float currentShakeDuration = 0f;

    void Awake()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake()
    {
        initialPosition = transform.localPosition;
        currentShakeDuration = shakeDuration;
    }
}