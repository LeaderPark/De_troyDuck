using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Processor
{
    Rigidbody rigidbody;
    
    public Move(Hashtable owner, Rigidbody rigidbody) : base(owner)
    {
        this.rigidbody = rigidbody;
    }

    private void MoveToWard(Vector3 normal, float power)
    {
        Vector3 velocity = normal * power;

        rigidbody.velocity = velocity;
    }
    
}
