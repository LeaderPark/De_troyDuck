using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEvent : EntityEvent
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
            Dash(inputX, inputY, 20f, 0.8f, 0.1f);
        }
        };
    }

}
