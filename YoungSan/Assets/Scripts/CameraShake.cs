using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static void Shake()
    {
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        Vector3 pos = virtualCam.transform.position;
        Quaternion rot = virtualCam.transform.rotation;
        virtualCam.ForceCameraPosition(pos + Vector3.right * Mathf.PerlinNoise(Random.Range(0, 100), 0) * 0.2f, rot);
    }
}
