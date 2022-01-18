using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    private Hashtable Processors {get; set;}

    [SerializeField] private EntityData entityData;
    public Clone clone;
    
    public Processor GetProcessor(Type processor)
    {
        if (Processors.ContainsKey(processor))
        {
            return Processors[processor] as Processor;
        }
        return null;
    }

    private void Process()
    {
        foreach (Processor processor in Processors.Values)
        {
            processor.Process();
        }
    }
    
    void Awake()
    {
        clone = new Clone(entityData);
        Processors = new Hashtable();
        SettingProcessor();
    }

    private void SettingProcessor()
    {
        if (GetComponent<Animator>() != null)
        {
            new Animate(Processors, GetComponent<Animator>());
        }
        if (GetComponent<Rigidbody>() != null)
        {
            new Move(Processors, GetComponent<Rigidbody>());
        }
        if (GetComponent<BoxCollider>() != null)
        {
            new Collision(Processors, GetComponent<BoxCollider>());
        }
    }

    void LateUpdate()
    {
        Process();
    }
}
