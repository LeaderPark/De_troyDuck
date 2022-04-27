using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Rain : MonoBehaviour
{
    private VisualEffect visualEffect;
    Coroutine updateRainPosition;

    void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
        updateRainPosition = StartCoroutine(UpdateRainPositionRoutine());
    }

    void OnEnable()
    {
        if (updateRainPosition != null) StopCoroutine(updateRainPosition);
        updateRainPosition = StartCoroutine(UpdateRainPositionRoutine());
    }

    IEnumerator UpdateRainPositionRoutine()
    {
        while (gameObject.activeSelf)
        {
            if (Camera.main != null)
            {
                visualEffect.SetVector3("PlayerPosition", Camera.main.transform.position);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
