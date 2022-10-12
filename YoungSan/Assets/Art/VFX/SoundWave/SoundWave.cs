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

        if (ownerEntity?.entityData.name == "Jing")
        {
            SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
            foreach (var sound in skillData.skillSet.skillDatas[EventCategory.Skill1][0].attackSounds)
            {
                soundManager.SoundStart(sound.name, transform);
            }
        }

        yield return new WaitForSeconds(0.1f);
        visualEffect.SetBool("Loop", false);
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<BoxCollider>().enabled = false;

        gameObject.SetActive(false);
    }
}
