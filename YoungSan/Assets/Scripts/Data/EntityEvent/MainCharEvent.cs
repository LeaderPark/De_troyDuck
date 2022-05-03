using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharEvent : EntityEvent
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
        maxAttackStack[EventCategory.DefaultAttack] = 3;
        attackProcess[EventCategory.DefaultAttack] = new AttackProcess[]{ 
        (inputX, inputY, position, skillData) =>
        {
            Vector2 cur = new Vector2(inputX, inputY);
            Projectile(cur.x, cur.y, "TestArrow", skillData, 0.06f);
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
            Defend(0f, 10f, 0.8f);
        },
        (inputX, inputY, position, skillData) =>
        {
            Projectile(inputX, inputY, "TestArrow", skillData, 0.06f);
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
        },
        (inputX, inputY, position, skillData) =>
        {
            Projectile(inputX, inputY, "TestArrow", skillData, 0.06f);
            Dash(inputX, inputY, entity.clone.GetStat(StatCategory.Speed) * 2, 0, 0.08f);
        }
        };
    }

    private void Skill1()
    {
        
        maxAttackStack[EventCategory.Skill1] = 1;
        attackProcess[EventCategory.Skill1] = new AttackProcess[]{
            null
        };
    }

    private void Skill2()
    {
        
        maxAttackStack[EventCategory.Skill2] = 1;
        attackProcess[EventCategory.Skill2] = new AttackProcess[]{
            null
        };
    }
}
