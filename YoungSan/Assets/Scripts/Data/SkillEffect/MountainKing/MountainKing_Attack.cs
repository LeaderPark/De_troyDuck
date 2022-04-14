using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKing_Attack : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        Stiff(hitEntity, 0.6f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.2f, 10);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.2f, 8);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        ChangeColor(hitEntity, Color.red, 0.1f);
    }
}
