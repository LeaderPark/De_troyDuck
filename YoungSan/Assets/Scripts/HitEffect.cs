using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public void Play(AnimationClip clip)
    {
        StartCoroutine(OffEffect(clip));
    }

    IEnumerator OffEffect(AnimationClip clip)
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(clip.name);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        animator.gameObject.SetActive(false);
    }
}
