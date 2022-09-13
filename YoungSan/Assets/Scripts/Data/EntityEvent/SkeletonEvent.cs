using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEvent : EntityEvent
{

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
    }

    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, 10f, 0.475f, 0.1f);
        }
        };
    }

}