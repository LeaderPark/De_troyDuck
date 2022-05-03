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
        attackIndex[EventCategory.DefaultAttack] = new int[]{ 0 };
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{ 
        null
        };
    }

}
