using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventTrigger<T>
where T : GlobalEvent
{
    
    private List<T> globalEvents;


    public void AddGlobalEvent(GlobalEvent globalEvent)
    {
        globalEvents.Add((T)globalEvent);
    }

    public void RemoveGlobalEvent(GlobalEvent globalEvent)
    {
        globalEvents.Remove((T)globalEvent);
    }
}
