using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKing_Sonic : MonoBehaviour
{
    const float startTime = 1f;
    const float time = 0.15f;

    public void Play(Entity entity, Vector2 direction)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().Play("MountainKing_Sonic", 0, 0f);
        transform.rotation = Quaternion.AngleAxis(Vector2.Dot(Vector2.right, direction), Vector3.up);
        StartCoroutine(Follow(entity));
    }

    IEnumerator Follow(Entity entity)
    {
        yield return new WaitForSeconds(startTime);
        GetComponent<SpriteRenderer>().enabled = true;

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
