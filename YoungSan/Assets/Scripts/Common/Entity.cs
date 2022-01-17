using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    protected Hashtable Processors {get; private set;}

    
    protected Processor GetProcessor(Type processor)
    {
        if (Processors.Contains(processor))
        {
            return Processors[processor] as Processor;
        }
        return null;
    }

    protected void Process()
    {
        foreach (Processor processor in Processors.Values)
        {
            processor.Process();
        }
    }
    
    void Awake()
    {
        Processors = new Hashtable();
    }

    void LateUpdate()
    {
        Process();
    }
}
