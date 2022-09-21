using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BukEvent : EntityEvent
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
            null
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
        (inputX, inputY, position, skillData) =>
        {
            Installation(entity.transform.position, skillData, "BukLurkerController", 0.5f);
        }
        };
    }

}