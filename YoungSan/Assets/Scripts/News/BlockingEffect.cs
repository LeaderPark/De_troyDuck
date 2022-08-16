using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingEffect : MonoBehaviour
{
    Entity entity;

    public void SetData(Entity entity)
    {
        this.entity = entity;
        transform.position = entity.transform.position + new Vector3(0, 1.3f, 0);
    }

    void Update()
    {
        if (entity == null)
        {
            gameObject.SetActive(false);
        }

        transform.position = entity.transform.position + new Vector3(0, 1.3f, 0);
    }
}
