using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Processor
{
    public class Animate : Processor
    {
        private Animator animator;
        
        public Animate(Hashtable owner, Animator animator) : base(owner)
        {
            this.animator = animator;
        }

        private void Play(string stateName)
        {
            if (Locker) return;
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

        private void PlayNoLock(string stateName)
        {
            animator.speed = 1f;
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

        void CheckClip(string stateName, System.Action<float> onClipEnd)
        {
            if (Locker) return;
            var animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(stateName))
            {
                onClipEnd(animatorState.normalizedTime);
            }
        }

        void CheckClipNoLock(string stateName, System.Action<float> onClipEnd)
        {
            var animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(stateName))
            {
                onClipEnd(animatorState.normalizedTime);
            }
        }

        protected override void StartLock()
        {
            animator.speed = 0f;
        }
        protected override void EndLock()
        {
            animator.speed = 1f;
        }

    }

}