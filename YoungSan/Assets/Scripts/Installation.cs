using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    Entity ownerEntity;


    public void SetData(Entity ownerEntity)
    {
        this.ownerEntity = ownerEntity;
    }
}
