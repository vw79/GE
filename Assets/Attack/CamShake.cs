using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    public static CamShake instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
    }

    // Modified CameraShake method to accept shake intensity as a parameter
    public void CameraShake(CinemachineImpulseSource impulseSource, float shakeIntensity)
    {
        impulseSource.GenerateImpulseWithForce(shakeIntensity);
    }
}
