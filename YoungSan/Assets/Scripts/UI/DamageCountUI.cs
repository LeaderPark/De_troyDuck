using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCountUI : MonoBehaviour
{
    public Transform clone;

    List<Transform> pool;

    void Awake()
    {
        pool = new List<Transform>();
    }

    public void Play(Vector3 position, int damage, bool isPlayer, bool isHeal)
    {
        int index = GetObject();
        TMPro.TextMeshProUGUI tmp = pool[index].GetComponent<TMPro.TextMeshProUGUI>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);

        tmp.text = damage.ToString();
        tmp.fontSize = 50;
        tmp.transform.position = screenPoint;

        tmp.color = Color.white;
        if (isPlayer)
        {
            tmp.color = Color.red;
        }
        if (isHeal)
        {
            tmp.color = Color.green;
        }

        StartCoroutine(Process(tmp, screenPoint));
    }

    IEnumerator Process(TMPro.TextMeshProUGUI tmp, Vector2 baseVec)
    {
        float timeStack = 0;
        int dir = Random.Range(0, 2);
        while (timeStack < 1f)
        {
            timeStack += Time.deltaTime;
            timeStack = Mathf.Clamp(timeStack, 0f, 1f);

            float x = timeStack * 5.1f;

            Vector2 moveVec;

            moveVec.y = 4 * -(Mathf.Pow(x - 2f, 2) - 2);
            if (dir == 0)
            {
                moveVec.x = x;
            }
            else
            {
                moveVec.x = -x;
            }

            tmp.fontSize = (int)Mathf.Lerp(50, 1, timeStack / 1f);
            tmp.transform.position = baseVec + moveVec * 10;

            yield return null;
        }
        tmp.gameObject.SetActive(false);
        yield return null;
    }

    int GetObject()
    {
        Debug.Assert(pool != null, "pool is null");


        int objectIndex = -1;

        for (int i = 0; i < pool.Count; i += 1)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                objectIndex = i;
            }
        }

        if (objectIndex == -1)
        {
            pool.Add(GameObject.Instantiate(clone.gameObject).transform);
            objectIndex = pool.Count - 1;
            pool[objectIndex].transform.parent = transform;
        }

        pool[objectIndex].gameObject.SetActive(true);

        return objectIndex;
    }
}
