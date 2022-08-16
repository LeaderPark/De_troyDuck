using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperArmourEffect : MonoBehaviour
{
    Entity entity;
    SpriteRenderer spriteRenderer;
    SpriteRenderer entitySr;

    public void SetData(Entity entity)
    {
        this.entity = entity;
        entitySr = entity.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = entity.transform.position + new Vector3(0, -0.1f, 0);
        transform.localScale = entity.transform.localScale + Vector3.one * 0.2f;
        spriteRenderer.sprite = entitySr.sprite;
        spriteRenderer.flipX = entitySr.flipX;
    }

    void Update()
    {
        if (entity == null)
        {
            gameObject.SetActive(false);
        }

        transform.position = entity.transform.position + new Vector3(0, -0.1f, 0);
        spriteRenderer.sprite = entitySr.sprite;
        spriteRenderer.flipX = entitySr.flipX;
    }
}
