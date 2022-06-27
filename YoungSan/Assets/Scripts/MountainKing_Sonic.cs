using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKing_Sonic : MonoBehaviour
{
    const float time = 0.15f;

    public void Play(Entity entity, Vector2 direction)
    {
        GetComponent<Animator>().Play("MountainKing_Sonic", 0, 0f);
        if (direction.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, new Vector3(direction.x, 0, direction.y)));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.right, new Vector3(direction.x, 0, direction.y)));
        }
        StartCoroutine(Follow(entity));
    }

    IEnumerator Follow(Entity entity)
    {
        float timeStack = 0;
        while (timeStack < time)
        {
            timeStack += Time.deltaTime;
            transform.position = entity.transform.position + Vector3.up * entity.entityData.uiPos / 3 + Vector3.back * 0.01f;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
