using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Processor
{
    public class Animate : Processor
    {
        private Animator animator;
        bool locking;
        
        public Animate(Hashtable owner, Animator animator) : base(owner)
        {
            this.animator = animator;
        }

        private void Play(string stateName)
        {
            lock(lockObject)
            {
                if (Locker) return;
            }
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

        void CheckClip(string stateName, System.Action<bool, float> onClipEnd)
        {
            lock(lockObject)
            {
                if (Locker) return;
            }
            var animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(stateName))
            {
                onClipEnd(false, animatorState.normalizedTime);
            }
            if (locking)
            {
                onClipEnd(true, 0f);
            }
        }

        void CheckClipNoLock(string stateName, System.Action<bool, float> onClipEnd)
        {
            var animatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorState.IsName(stateName))
            {
                onClipEnd(false, animatorState.normalizedTime);
            }
            if (locking)
            {
                onClipEnd(true, 0f);
            }
        }

        protected override void StartLock()
        {
            animator.speed = 0f;
            locking = true;
        }
        protected override void EndLock()
        {
            animator.speed = 1f;
            locking = false;
        }

    }

}