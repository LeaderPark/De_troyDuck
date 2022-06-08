using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public int playCheck;

    public IEnumerator Play()
    {
        playCheck = 0;
        Animator animator = GetComponent<Animator>();
        animator.Play("Cloud");

        while (playCheck != 1)
        {
            yield return null;
        }
    }

    public IEnumerator End()
    {
        while (playCheck != 2)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
