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
        TriggerSetting();
    }

    void TriggerSetting()
    {
        GetEventTrigger(typeof(StatEventTrigger)).Add(new GlobalEventTrigger.StatEvent((entity, category, oldValue, value) =>
        {
            if (category == StatCategory.Stamina)
            {
                if (value < oldValue)
                {
                    entity.ResetStaminaCount();
                }
            }
        }));
    }
}
