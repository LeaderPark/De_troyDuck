using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatEventTrigger : GlobalEventTrigger
{

    public void Add(StatEvent action)
    {
        onTrigger = System.Delegate.Combine(onTrigger, action);
    }

    public void Remove(StatEvent action)
    {
        onTrigger = System.Delegate.Remove(onTrigger, action);
    }
}
