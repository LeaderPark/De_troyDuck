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
                if (LockTimer <= 0)
                {
                    if (LockTimer == -1) return;
                    Locker = false;
                    EndLock();
                }
                LockTimer -= Time.deltaTime;
            }
        }

        protected bool Locker;
        private float LockTimer;

        protected void LockTime(float time)
        {
            LockTimer = time;
            StartLock();

            if (!Locker)
            {
                Locker = true;
            }
        }

        protected void Lock()
        {
            LockTimer = -1;
            StartLock();
            Locker = true;
        }

        protected void UnLock()
        {
            LockTimer = 0;
            Locker = false;
            EndLock();
        }

        protected virtual void StartLock()
        {

        }

        protected virtual void EndLock()
        {

        }
    }
}
