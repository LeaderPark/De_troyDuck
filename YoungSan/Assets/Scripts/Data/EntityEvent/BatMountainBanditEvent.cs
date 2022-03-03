using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMountainBanditEvent : EntityEvent
{

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackAnimation[EventCategory.DefaultAttack] = new string[]{ "Attack" };
        attackTransitionTime[EventCategory.DefaultAttack] = new (float, float)[]{};
        attackIndex[EventCategory.DefaultAttack] = new int[]{ 0 };
        attackProcess[EventCategory.DefaultAttack] = new System.Action<float, float>[]{ 
        null
        };
    }

}
