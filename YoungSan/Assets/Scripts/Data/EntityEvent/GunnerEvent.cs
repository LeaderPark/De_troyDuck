using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerEvent : EntityEvent
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
            Dash(inputX, inputY, -10, 0.1f, 0.1f);
            Vector2 cur = new Vector2(inputX, inputY);
            Projectile(cur.x, cur.y, "Bullet", skillData, 0.1f);
        }
        };
    }

}