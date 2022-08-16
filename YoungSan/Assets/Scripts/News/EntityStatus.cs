using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus
{
    private bool activated;

    private float activateTime;

    public void OnUpdate()
    {
        if (activated)
        {
            if (activateTime > 0)
            {
                activateTime -= Time.deltaTime;

                if (activateTime <= 0)
                {
                    DeActivate();
                }
            }
        }
    }

    public bool Activated()
    {
        return activated;
    }

    public virtual void Activate()
    {
        if (activated)
        {
            DeActivate();
        }

        activateTime = -1;
        activated = true;
    }

    public virtual void DeActivate()
    {
        if (activated)
        {
            activateTime = 0;
            activated = false;
        }
    }

    public void ActivateForTime(float time)
    {
        Activate();
        activateTime = time;
    }
}
