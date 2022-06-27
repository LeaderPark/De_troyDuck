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
        Skill2();
    }
    private void DefalutAttack()
    {
        maxAttackStack[EventCategory.DefaultAttack] = 1;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) Defend(0f, skillData.skill.length, 0f);
        }
        };
    }

    private void Skill1()
    {
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) Defend(0f, skillData.skill.length, 0f);
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 16
                , 1f, 0.15f);
        }
        };
    }

    private void Skill2()
    {
        maxAttackStack[EventCategory.Skill2] = 1;
        attackProcess[EventCategory.Skill2] = new AttackProcess[]{
        (inputX, inputY, position, skillData) =>
        {
            if (skillData.skillSet.entity.gameObject.CompareTag("Boss")) Defend(0f, skillData.skill.length, 0f);
        }
        };
    }
}
