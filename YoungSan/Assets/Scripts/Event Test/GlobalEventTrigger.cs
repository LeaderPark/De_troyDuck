using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventTrigger : MonoBehaviour
{
    public delegate void GlobalEvent();
    public delegate void StatEvent(Entity entity, int oldVale, int value);
    public delegate void HitEvent(Entity hitEntity, Entity attackEntity, int damage);
    public delegate void DieEvent(Entity hitEntity, Entity attackEntity);
    public delegate void SkillEvent(Entity entity, SkillData skillData);
    
    private System.Delegate onTrigger;


    public void Add(System.Delegate action)
    {
        onTrigger = System.Delegate.Combine(onTrigger, action);
    }

    public void Remove(System.Delegate action)
    {
        onTrigger = System.Delegate.Remove(onTrigger, action);
    }

    public void Clear()
    {
        foreach (var item in onTrigger.GetInvocationList())
        {
            Remove(item);
        }
    }

    public void Invoke(params object[] args)
    {
        onTrigger.DynamicInvoke(args);
    }
}

public enum AreaEventState
{
    Enter,
    Stay,
    Exit
}
