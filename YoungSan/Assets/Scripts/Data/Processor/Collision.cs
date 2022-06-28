using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class Collision : Processor
    {
        BoxCollider collider;
        SpriteRenderer spriteRenderer;
        public Collision(Hashtable owner, BoxCollider collider) : base(owner)
        {
            this.collider = collider;
            spriteRenderer = collider.GetComponent<SpriteRenderer>();
        }

        private void SetCollider(UnityEngine.Sprite sprite)
        {
# if USE_COLLISION
            Vector2[] vertices = sprite.vertices;
            Rect rect = sprite.rect;

            Vector2 size = new Vector2(rect.width, rect.height) / (sprite.pixelsPerUnit * 2);


            float minX = vertices[0].x;
            float maxX = vertices[0].x;
            float minY = vertices[0].y;
            float maxY = vertices[0].y;

            foreach (var item in vertices)
            {
                if (minX > item.x)
                {
                    minX = item.x;
                }
                if (maxX < item.x)
                {
                    maxX = item.x;
                }
                if (minY > item.y)
                {
                    minY = item.y;
                }
                if (maxY < item.y)
                {
                    maxY = item.y;
                }
            }

            Vector3 verSize = new Vector3(maxX - minX, maxY - minY);

            collider.size = new Vector3(verSize.x, verSize.y, collider.size.z);

            Vector3 pivot = verSize / 2 + new Vector3(minX, minY,collider.center.z);

            collider.center = pivot;
#endif

            collider.center = new Vector3((spriteRenderer.flipX ? -1 : 1) * Mathf.Abs(collider.center.x), collider.center.y, collider.center.z);
        }
    }
}
