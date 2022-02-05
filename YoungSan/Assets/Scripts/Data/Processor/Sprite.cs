using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class Sprite : Processor
    {
        SpriteRenderer spriteRenderer;
        public Sprite(Hashtable owner, SpriteRenderer spriteRenderer) : base(owner)
        {
            this.spriteRenderer = spriteRenderer;
        }

        private void SetDirection(bool isRight)
        {
            lock(lockObject)
            {
                if (Locker) return;
            }
            spriteRenderer.flipX = !isRight;
        }

        private void SetColor(Color color)
        {
            spriteRenderer.material.SetColor("_Color", color);
        }
    }
}
