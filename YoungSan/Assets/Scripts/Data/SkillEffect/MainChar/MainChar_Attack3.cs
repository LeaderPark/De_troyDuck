using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack3 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.white, 0.1f);
        KnockBack(hitEntity, direction, 0.1f, 8);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.white, 0.1f);
        KnockBack(hitEntity, direction, 0.1f, 10);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        ChangeColor(hitEntity, Color.white, 0.1f);
    }
}