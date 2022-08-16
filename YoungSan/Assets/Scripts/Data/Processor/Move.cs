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

            RaycastHit hit;

            Vector3 pos = rigidbody.transform.position;
            if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
            {
                pos.y = hit.point.y;
                rigidbody.transform.position = pos;
            }

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

            RaycastHit hit;

            Vector3 pos = rigidbody.transform.position;
            if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
            {
                pos.y = hit.point.y;
                rigidbody.transform.position = pos;
            }

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
