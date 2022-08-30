using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Processor
{
    public class Move : Processor
    {
        Rigidbody rigidbody;

        public Move(Hashtable owner, Rigidbody rigidbody) : base(owner)
        {
            this.rigidbody = rigidbody;
        }

        private void SetVelocity(Vector3 normal, float power)
        {
            if (Locker) return;
            Entity entity = rigidbody.GetComponent<Entity>();
            if (entity.entityStatusAilment != null)
            {
                if (entity.entityStatusAilment.GetEntityStatus(typeof(Airbone)).Activated()) return;
            }
            Vector3 velocity = normal * power;

            rigidbody.velocity = velocity;
        }

        private void SetVelocityNoLock(Vector3 normal, float power)
        {
            Entity entity = rigidbody.GetComponent<Entity>();
            if (entity.entityStatusAilment != null)
            {
                if (entity.entityStatusAilment.GetEntityStatus(typeof(Airbone)).Activated()) return;
            }
            Vector3 velocity = normal * power;

            rigidbody.velocity = velocity;
        }


        protected override void StartLock()
        {

        }
        protected override void EndLock()
        {

        }

    }
}
