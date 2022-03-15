using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Manager
{
    private Hashtable eventTriggerTable;

    public GlobalEventTrigger GetEventTrigger(System.Type type)
    {
        if (eventTriggerTable.ContainsKey(type))
        {
            return eventTriggerTable[type] as GlobalEventTrigger;
        }
        return null;
    }

    void Awake()
    {
        eventTriggerTable = new Hashtable();
        eventTriggerTable.Add(typeof(StatEventTrigger), new StatEventTrigger());
        eventTriggerTable.Add(typeof(HitEventTrigger), new HitEventTrigger());
        eventTriggerTable.Add(typeof(DieEventTrigger), new DieEventTrigger());
        eventTriggerTable.Add(typeof(SkillEventTrigger), new SkillEventTrigger());
    }
}
