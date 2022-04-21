using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharEvent : EntityEvent
{

    protected override void Awake()
    {
        base.Awake();
        DefalutAttack();
    }

    
    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 3;
        attackIndex[EventCategory.DefaultAttack] = new int[]{ 0, 1, 2 };
        attackProcess[EventCategory.DefaultAttack] = new System.Action<float, float>[]{ 
        (inputX, inputY) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
        },
        (inputX, inputY) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
        },
        (inputX, inputY) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
        }
        };
    }
}
