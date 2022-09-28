using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusAilment : MonoBehaviour
{
    private Hashtable entityStatus;

    public EntityStatus GetEntityStatus(System.Type type)
    {
        if (entityStatus.ContainsKey(type))
        {
            return entityStatus[type] as EntityStatus;
        }
        return null;
    }

    void Awake()
    {
        entityStatus = new Hashtable();

        entityStatus.Add(typeof(Fainting), new Fainting());
        entityStatus.Add(typeof(Igniting), new Igniting());
        entityStatus.Add(typeof(Poisoning), new Poisoning());
        entityStatus.Add(typeof(Bleeding), new Bleeding());
        entityStatus.Add(typeof(SuperArmour), new SuperArmour());
        entityStatus.Add(typeof(Defending), new Defending());
        entityStatus.Add(typeof(Blocking), new Blocking());
        entityStatus.Add(typeof(Airbone), new Airbone());
    }

    void Update()
    {
        foreach (EntityStatus status in entityStatus.Values)
        {
            status.OnUpdate();
        }
    }

    public void DeActiveAll()
    {
        foreach (EntityStatus status in entityStatus.Values)
        {
            status.DeActivate();
        }
    }
}

public enum TickAilment
{
    Igniting,
    Poisoning,
    Bleeding,
}