using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKingEvent : EntityEvent
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
        attackIndex[EventCategory.DefaultAttack] = new int[] { 0 };
        attackProcess[EventCategory.DefaultAttack] = new System.Action<float, float>[]{
        null
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackIndex[EventCategory.Skill1] = new int[] { 0 };
        attackProcess[EventCategory.Skill1] = new System.Action<float, float>[]{
        (inputX, inputY) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 16
                , 1f, 0.15f);
        }
        };
    }
}
