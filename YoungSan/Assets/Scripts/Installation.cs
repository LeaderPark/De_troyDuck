using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    Entity ownerEntity;
    Vector3 position;


    public void SetData(Entity ownerEntity, Vector3 position)
    {
        this.ownerEntity = ownerEntity;
        this.position = position;
    }
}
