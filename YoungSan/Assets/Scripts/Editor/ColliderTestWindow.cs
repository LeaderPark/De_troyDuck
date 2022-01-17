using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColliderTestWindow : EditorWindow
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider collider;

    void OnGUI()
    {
        spriteRenderer = EditorGUILayout.ObjectField(spriteRenderer, typeof(SpriteRenderer), true) as SpriteRenderer;
        collider = EditorGUILayout.ObjectField(collider, typeof(BoxCollider), true) as BoxCollider;

        if (GUILayout.Button("Setting"))
        {
            ColliderTesting();
        }
    }

    
    void ColliderTesting()
    {
        Vector2[] vertices = spriteRenderer.sprite.vertices;
        Rect rect = spriteRenderer.sprite.rect;
        
        Vector2 size = new Vector2(rect.width, rect.height) / spriteRenderer.sprite.pixelsPerUnit;

        Debug.Log(size.x + " " + size.y);

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
        
        collider.size = new Vector3(maxX - minX, maxY - minY, collider.size.z);
    }
}
