using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 shakeOffset;
    private float shakeDuration;
    private float shakeMagnitude;

    private void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0; // keep shake in x-y, not depth
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        // Apply shake *after* your follow script has moved the camera
        transform.localPosition += shakeOffset;
    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
