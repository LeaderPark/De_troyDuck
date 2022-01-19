using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processor
{
    public class Collision : Processor
    {
        BoxCollider collider;
        public Collision(Hashtable owner, BoxCollider collider) : base(owner)
        {
            this.collider = collider;
        }

        private void SetCollider(Sprite sprite)
        {
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

            Vector2 verSize = new Vector2(maxX - minX, maxY - minY);
            
            collider.size = new Vector3(verSize.x, verSize.y, collider.size.z);

            Vector2 pivot = verSize / 2 + new Vector2(minX, minY);

            collider.center = pivot;
        }
    }
}
