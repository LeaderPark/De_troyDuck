using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveEvent : EntityEvent
{

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
        Skill1();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 2;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0.1f, 0.1f);
        },
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0.2f, 0.1f);
        },
        };
    }
    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 3f, 0.714f, 2f);
        },
        };
    }

}