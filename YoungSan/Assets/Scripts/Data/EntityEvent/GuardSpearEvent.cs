using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpearEvent : EntityEvent
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
            Flash(position, 10f, 0f);
            Dash(inputX, inputY, 8, 0.4f, 0.4f);
        }
        };
    }

}