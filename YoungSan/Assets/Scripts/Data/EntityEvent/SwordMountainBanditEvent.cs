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
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        null,
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 4, 0.1f, 0.08f);
        }
        };
    }

}
