using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.1f, 8);
        Grab(attackEntity, hitEntity, 40, 0f, 1f);
        TickDamage("Poison", attackEntity, hitEntity, 1, 5);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.2f, 8);
        Grab(attackEntity, hitEntity, 40, 0f, 1f);
        TickDamage("Poison", attackEntity, hitEntity, 1, 5);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0.1f);
        Grab(attackEntity, hitEntity, 40, 0f, 1f);
        TickDamage("Poison", attackEntity, hitEntity, 1, 5);
    }
}
