using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Attack : SkillEffect
{

    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        KnockBack(hitEntity, direction, 0f, 0.1f, 8);
        TickDamage("Poison", attackEntity, hitEntity, 0.2f, 1f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        KnockBack(hitEntity, direction, 0f, 0.2f, 8);
        TickDamage("Poison", attackEntity, hitEntity, 0.2f, 1f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        TickDamage("Poison", attackEntity, hitEntity, 0.2f, 1f);
    }
}
