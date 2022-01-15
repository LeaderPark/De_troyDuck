using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Hashtable processors;

    protected Hashtable Processors {get; private set;}

    
    protected Processor GetProcessor(string processorName)
    {
        if (Processors.Contains(processorName))
        {
            return Processors[processorName] as Processor;
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
