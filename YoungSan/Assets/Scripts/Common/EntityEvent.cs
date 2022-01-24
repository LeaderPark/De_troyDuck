using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvent : MonoBehaviour
{
    public Entity entity;
    public void CallEvent(EventCategory eventCategory, object[] parameters)
    {
        this.GetType().GetMethod(string.Concat("Call", eventCategory.ToString()), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, parameters);
    }

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
    }
}

public enum EventCategory
{
    Move,
    DefaultAttack,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Skill5,
    Skill6
}
