using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Processor
{
    public class Processor
    {
        private List<(string, object[])> commands;

        public Processor(Hashtable owner)
        {
            commands = new List<(string, object[])>();
            owner.Add(this.GetType(), this);
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

            if (LockTimer != null && !Locker)
            {
                LockTimer = null;
                EndLock();
            }
        }
        
        protected bool Locker;
        
        private Timer LockTimer;

        protected void Lock(float time)
        {
            if (LockTimer != null) LockTimer.Dispose();
            Locker = true;
            StartLock();
            LockTimer = new Timer((o) =>
            {
                Locker = false;
                LockTimer.Dispose();
            }, null, (int)(time * 1000), Timeout.Infinite);
        }

        protected virtual void StartLock()
        {

        }

        protected virtual void EndLock()
        {

        }
    }
}
