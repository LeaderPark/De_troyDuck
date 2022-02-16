using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    public static CameraShake Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new GameObject("Camera Shake").AddComponent<CameraShake>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public void Shake()
    {
        CinemachineVirtualCamera virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        StartCoroutine(ShakeRepeat(virtualCam, 2, 0.1f));
    }

    IEnumerator ShakeRepeat(CinemachineVirtualCamera virtualCam, int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = virtualCam.transform.position;
            Quaternion rot = virtualCam.transform.rotation;
            virtualCam.ForceCameraPosition(pos + new Vector3(Random.Range(0.0f, 1.0f) - 0.5f, Random.Range(0.0f, 1.0f) - 0.5f, 0), rot);
            yield return new WaitForSeconds(delay);
        }
    }
}
