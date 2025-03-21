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
        transform.position = entity.transform.position;
        transform.localScale = entity.transform.localScale;
        spriteRenderer.sprite = entitySr.sprite;
        spriteRenderer.flipX = entitySr.flipX;
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        while (true)
        {
            if (entity == null)
            {
                gameObject.SetActive(false);
                yield break;
            }

            if (entity) transform.position = entity.transform.position;
            if (entity) spriteRenderer.sprite = entitySr.sprite;
            if (entity) spriteRenderer.flipX = entitySr.flipX;

            yield return null;
        }
    }
}
