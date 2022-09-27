using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SoundWave : Installation
{
    public VisualEffect visualEffect;

    public override void Play()
    {
        GetComponent<HitBox>().ClearTargetSet();
        GetComponent<HitBox>().skillData = skillData;

        transform.position = position;

        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        visualEffect.SetBool("Loop", true);
        yield return new WaitForSeconds(0.1f);
        visualEffect.SetBool("Loop", false);
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider>().enabled = false;

        gameObject.SetActive(false);
    }
}
