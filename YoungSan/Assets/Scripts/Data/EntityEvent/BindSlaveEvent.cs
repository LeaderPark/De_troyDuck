using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindSlaveEvent : EntityEvent
{
    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackIndex[EventCategory.DefaultAttack] = new int[] { 0 };
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 3f, 0, 0.7f);
        }
        };
    }
}
