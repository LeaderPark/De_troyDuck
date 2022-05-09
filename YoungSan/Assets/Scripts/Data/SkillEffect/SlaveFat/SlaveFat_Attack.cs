using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveFat_Attack : SkillEffect
{

    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        switch (index)
        {
            case 0:
                Stiff(hitEntity, 0.2f);
                KnockBack(hitEntity, direction, 0f, 0.2f, 2);
                ChangeColor(hitEntity, Color.red, 0f, 0.1f);
            break;
            case 1:
                Stiff(hitEntity, 0.2f);
                KnockBack(hitEntity, direction, 0f, 0.2f, 2);
                ChangeColor(hitEntity, Color.red, 0f, 0.1f);
            break;
        }
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        switch (index)
        {
            case 0:
                Stiff(hitEntity, 0.2f);
                KnockBack(hitEntity, direction, 0f, 0.2f, 2);
                ChangeColor(hitEntity, Color.red, 0f, 0.1f);
            break;
            case 1:
                Stiff(hitEntity, 0.2f);
                KnockBack(hitEntity, direction, 0f, 0.2f, 2);
                ChangeColor(hitEntity, Color.red, 0f, 0.1f);
            break;
        }
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}