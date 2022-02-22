using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    private Hashtable Processors {get; set;}

    public EntityData entityData;
    public Clone clone;

    public bool isDead;
        
    public Processor.Processor GetProcessor(Type processor)
    {
        if (Processors.ContainsKey(processor))
        {
            return Processors[processor] as Processor.Processor;
        }
        return null;
    }

    private void Process()
    {
        foreach (Processor.Processor processor in Processors.Values)
        {
            processor.Process();
        }
    }
    
    void Awake()
    {
        Processors = new Hashtable();
        clone = new Clone(this, entityData);
        isDead = false;
        SettingProcessor();
    }

    private void SettingProcessor()
    {
        if (GetComponent<Animator>() != null)
        {
            new Processor.Animate(Processors, GetComponent<Animator>());
        }
        if (GetComponent<Rigidbody>() != null)
        {
            new Processor.Move(Processors, GetComponent<Rigidbody>());
        }
        if (GetComponent<BoxCollider>() != null)
        {
            new Processor.Collision(Processors, GetComponent<BoxCollider>());
            new Processor.HitBody(Processors, clone);
        }
        if (GetComponentInChildren<SkillSet>() != null)
        {
            new Processor.Skill(Processors, GetComponentInChildren<SkillSet>());
        }
        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            new Processor.Sprite(Processors, GetComponent<SpriteRenderer>());
        }
    }

    void LateUpdate()
    {
        Process();
    }
}
