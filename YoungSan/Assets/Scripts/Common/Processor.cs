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
            lockObject = new object();
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

            lock(lockObject)
            {
                if (Locker)
                {
                    LockTimer -= Time.deltaTime;
                    if (LockTimer <= 0)
                    {
                        Locker = false;
                        EndLock();
                    }
                }
            }
        }
        
        protected bool Locker;
        private float LockTimer;
        protected object lockObject;

        protected void Lock(float time)
        {
            lock(lockObject)
            {
                if (!Locker)
                {
                    LockTimer = time;
                    StartLock();
                    Locker = true;
                }
            }
        }

        protected virtual void StartLock()
        {

        }

        protected virtual void EndLock()
        {

        }
    }
}
