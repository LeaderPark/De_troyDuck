using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveFatEvent : EntityEvent
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
        null,
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, 8, 0.125f, 0.1f);
        }
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Heal(0.9f, 1f, 0.2f, 0.4f);
        }
        };
    }

}