using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HevySmoke : MonoBehaviour
{

    private VisualEffect visualEffect;
    void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    public void Play()
    {
        visualEffect.SendEvent("OnPlay");
        StartCoroutine(OffEffect());
    }

    IEnumerator OffEffect()
    {
        yield return new WaitForSeconds(50f);
        gameObject.SetActive(false);
    }
}
