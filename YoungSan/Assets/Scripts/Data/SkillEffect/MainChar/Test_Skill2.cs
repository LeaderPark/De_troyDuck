using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Skill2 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        KnockBack(hitEntity, direction, 0f, 0.5f, 40);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        KnockBack(hitEntity, direction, 0f, 0.5f, 40);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        KnockBack(hitEntity, direction, 0f, 0.5f, 40);
    }
}