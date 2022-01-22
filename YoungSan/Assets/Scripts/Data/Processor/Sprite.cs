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
            if (Locker) return;
            spriteRenderer.flipX = !isRight;
        }
    }
}
