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
        attackAnimation[EventCategory.DefaultAttack] = new string[]{ "Attack1", "Attack2" };
        attackTransitionTime[EventCategory.DefaultAttack] = new (float, float)[]{ (0.6f, 0.9f) };
        attackIndex[EventCategory.DefaultAttack] = new int[]{ 0, 1 };
        attackProcess[EventCategory.DefaultAttack] = new System.Action<float, float>[]{ 
        null,
        (inputX, inputY) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 4, 0, 0.08f);
        }
        };
    }

}
