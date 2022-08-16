using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defending : EntityStatus
{
    private float rate;
    private int value;

    public void SetData(float rate, int value)
    {
        this.rate = rate;
        this.value = value;
    }

    public int GetData(Entity entity, int damage)
    {
        return (int)(damage * (1 - rate) - value);
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }
}
