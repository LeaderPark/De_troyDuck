using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEventTrigger : GlobalEventTrigger
{

    public void Add(DieEvent action)
    {
        onTrigger = System.Delegate.Combine(onTrigger, action);
    }

    public void Remove(DieEvent action)
    {
        onTrigger = System.Delegate.Remove(onTrigger, action);
    }
}
