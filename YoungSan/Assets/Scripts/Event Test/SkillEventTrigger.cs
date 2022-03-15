using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEventTrigger : GlobalEventTrigger
{

    public void Add(SkillEvent action)
    {
        onTrigger = System.Delegate.Combine(onTrigger, action);
    }

    public void Remove(SkillEvent action)
    {
        onTrigger = System.Delegate.Remove(onTrigger, action);
    }
}
