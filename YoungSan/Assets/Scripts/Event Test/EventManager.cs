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

        }
        return null;
    }
}
