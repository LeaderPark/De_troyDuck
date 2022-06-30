using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void Play(AnimationClip clip)
    {
        StartCoroutine(OffEffect(clip));
    }

    public void Play(string clip)
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

    IEnumerator OffEffect(string clip)
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(clip);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        animator.gameObject.SetActive(false);
    }
}
