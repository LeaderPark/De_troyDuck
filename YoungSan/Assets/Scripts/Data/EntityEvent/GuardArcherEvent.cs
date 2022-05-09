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
            Flash(entity.transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)), 10f, 0f);
            Vector2 cur = new Vector2(inputX, inputY);
            Projectile(cur.x, cur.y, "TestArrow", skillData, 1.3f);
        }
        };
    }

}