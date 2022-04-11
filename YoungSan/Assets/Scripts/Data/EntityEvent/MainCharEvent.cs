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
        attackAnimation[EventCategory.DefaultAttack] = new string[]{ "Attack1", "Attack2", "Attack3" };
        attackTransitionTime[EventCategory.DefaultAttack] = new (float, float)[]{ (0.4f, 0.9f), (0.4f, 0.9f) };
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
