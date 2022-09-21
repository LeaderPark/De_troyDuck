using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BukLurkerController : Installation
{
    List<BukLurker> pool = new List<BukLurker>();

    public GameObject clone;

    public float radius;
    public float interval;
    public float rotateInterval;
    public float timeInterval;


    BukLurker GetBukLurker()
    {
        BukLurker lurker = null;

        foreach (var item in pool)
        {
            if (!item.gameObject.activeSelf)
            {
                lurker = item;
            }
        }

        if (lurker == null)
        {
            lurker = Instantiate(clone, transform).GetComponent<BukLurker>();
            pool.Add(lurker);
        }

        lurker.gameObject.SetActive(true);

        return lurker;
    }

    public override void Play()
    {
        transform.position = position;

        StartCoroutine(ControlRoutine());
    }

    IEnumerator ControlRoutine()
    {
        int count = (int)(radius / interval);
        for (int c = 0; c < count; c++)
        {
            for (float r = 0; r < 360; r += rotateInterval)
            {
                Vector3 point = transform.position + Quaternion.AngleAxis(r, Vector3.up) * Vector3.forward * (c + 1) * interval;
                GetBukLurker().SetData(point, skillData);
            }
            yield return new WaitForSeconds(timeInterval);
        }
    }
}
