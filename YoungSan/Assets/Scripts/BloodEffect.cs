using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloodEffect : MonoBehaviour
{
    private VisualEffect visualEffect;

    void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    public void Play()
    {
        visualEffect.SendEvent("OnPlay");
        
    }

    IEnumerator OffEffect()
    {
        yield return new WaitForSeconds(40f);
        gameObject.SetActive(false);
    }
}
