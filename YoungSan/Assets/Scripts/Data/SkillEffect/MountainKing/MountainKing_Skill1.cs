using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainKing_Skill1 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.8f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.1f, 60);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0.1f);
        KnockBack(hitEntity, direction, 0.2f, 20);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0.1f);
    }
}
