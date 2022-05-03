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
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        null
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 16
                , 1f, 0.15f);
        }
        };
    }
}
