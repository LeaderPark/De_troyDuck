using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airbone : EntityStatus
{
    Entity entity;
    Vector3 power;


    public void SetData(Entity entity, Vector3 power)
    {
        this.entity = entity;
        this.power = power;
    }

    public override void Activate()
    {
        base.Activate();

        Rigidbody rigidBody = entity.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.velocity = Vector3.zero;

        rigidBody.AddForce(power, ForceMode.Impulse);

        entity.entityStatusAilment.StartCoroutine(CheckGround());
    }

    public override void DeActivate()
    {
        base.DeActivate();

        Rigidbody rigidBody = entity.GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
    }

    IEnumerator CheckGround()
    {
        yield return null;

        while (!entity.isGround)
        {
            yield return null;
        }
        DeActivate();
    }
}
