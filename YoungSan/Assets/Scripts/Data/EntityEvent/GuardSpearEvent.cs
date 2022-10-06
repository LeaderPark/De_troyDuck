using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpearEvent : EntityEvent
{

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
        Skill1();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, 8, 0.45f, 0.4f);
        }
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, 12, 0.27f, 0.4f);
        }
        };
    }

}