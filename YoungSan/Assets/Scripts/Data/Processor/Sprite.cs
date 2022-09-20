using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class Sprite : Processor
    {
        SpriteRenderer spriteRenderer;
        public bool locking;

        public Sprite(Hashtable owner, SpriteRenderer spriteRenderer) : base(owner)
        {
            this.spriteRenderer = spriteRenderer;
        }

        private void SetDirection(bool isRight)
        {
            if (Locker) return;
            spriteRenderer.flipX = !isRight;
        }

        private void SetColor(Color color)
        {
            spriteRenderer.material.SetColor("_Color", color);
        }

        protected override void StartLock()
        {
            locking = true;
        }
        protected override void EndLock()
        {
            locking = false;
        }
    }
}
