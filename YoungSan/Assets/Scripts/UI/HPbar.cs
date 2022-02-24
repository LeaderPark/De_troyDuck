using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbar : MonoBehaviour
{
    private Entity entity;
    void Start()
    {
        entity = GameObject.Find("MainChar").GetComponent<Entity>();
    }

    void Update()
    {
        //Debug.Log(entity.clone.GetStat(StatCategory.Health));
    }
}
