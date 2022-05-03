using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMountainBanditEvent : EntityEvent
{    

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 2;
        attackIndex[EventCategory.DefaultAttack] = new int[]{ 0, 1 };
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{ 
        null,
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 4, 0, 0.08f);
        }
        };
    }

}
