using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Processor
{
    private List<(string, object[])> commands;

    public Processor(Hashtable owner)
    {
        commands = new List<(string, object[])>();
        owner.Add(this.GetType().Name, this);
    }

    public void AddCommand(string command, object[] parameters)
    {
        commands.Add((command, parameters));
    }

    public void Process()
    {
        for (int i = 0; i < commands.Count; i++)
        {
            this.GetType().GetMethod(commands[i].Item1, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, commands[i].Item2);
        }
        commands.Clear();
    }
}
