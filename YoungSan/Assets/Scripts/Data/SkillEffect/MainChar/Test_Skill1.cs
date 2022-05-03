using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Skill1 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        Grab(attackEntity, hitEntity, 100, 0f, 1f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        Grab(attackEntity, hitEntity, 100, 0f, 1f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 2f);
        Grab(attackEntity, hitEntity, 100, 0f, 1f);
    }
}