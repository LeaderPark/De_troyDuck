using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardArcherEvent : EntityEvent
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
            Vector2 cur = new Vector2(inputX, inputY);
            Projectile(cur.x, cur.y, "Arrow", skillData, entity.transform.position, true, 1f);
        }
        };
    }

}