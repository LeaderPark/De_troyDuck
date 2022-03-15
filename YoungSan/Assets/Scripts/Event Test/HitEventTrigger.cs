using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEventTrigger : GlobalEventTrigger
{

    public void Add(HitEvent action)
    {
        onTrigger = System.Delegate.Combine(onTrigger, action);
    }

    public void Remove(HitEvent action)
    {
        onTrigger = System.Delegate.Remove(onTrigger, action);
    }
}
