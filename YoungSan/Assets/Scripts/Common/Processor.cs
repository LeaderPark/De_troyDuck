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

        public virtual void Process()
        {
            for (int i = 0; i < commands.Count; i++)
            {
                this.GetType().GetMethod(commands[i].Item1, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(this, commands[i].Item2);
            }
            commands.Clear();

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

        protected bool Locker;
        private float LockTimer;

        protected void Lock(float time)
        {
            if (!Locker)
            {
                if (LockTimer < time) LockTimer = time;
                StartLock();
                Locker = true;
            }
            else
            {
                LockTimer = time;
                StartLock();
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
