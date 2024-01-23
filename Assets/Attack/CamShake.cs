using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CamShake : MonoBehaviour
{
    public static CamShake instance;

    [SerializeField] private float globalShakeForce1 = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }   
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce1);
    }
}
