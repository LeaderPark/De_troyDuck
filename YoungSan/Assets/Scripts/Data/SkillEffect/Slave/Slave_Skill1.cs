using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slave_Skill1 : SkillEffect
{

    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.5f);
        KnockBack(hitEntity, direction, 0f, 0.2f, 5.5f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.5f);
        KnockBack(hitEntity, direction, 0f, 0.2f, 5.5f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowWallEffect(Entity attackEntity, Vector2 direction, int index)
    {
        Stiff(attackEntity, 4f);
        ChangeColor(attackEntity, Color.red, 0f, 0.1f);
    }
}