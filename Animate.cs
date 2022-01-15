using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : Processor
{
    private Animator animator;
    
    public Animate(Hashtable owner, Animator animator) : base(owner)
    {
        this.animator = animator;
    }

    private void Play(string stateName)
    {
        var animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName(stateName))
        {
            if (animatorState.normalizedTime >= 0.9f)
            {
                animator.Play(stateName);
            }
        }
        else
        {
            animator.Play(stateName);
        }
    }

}
