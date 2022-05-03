using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaveSoldier_Attack : SkillEffect
{

    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        switch (index)
        {
            case 0:
            case 1:
            case 2:
            Stiff(hitEntity, 0.2f);
            KnockBack(hitEntity, direction, 0f, 0f, 0);
            break;
            case 3:
            Stiff(hitEntity, 0.5f);
            KnockBack(hitEntity, direction, 0f, 0.1f, 12);
            break;
        }
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        switch (index)
        {
            case 0:
            case 1:
            case 2:
            Stiff(hitEntity, 0.4f);
            KnockBack(hitEntity, direction, 0f, 0f, 0);
            break;
            case 3:
            Stiff(hitEntity, 0.8f);
            KnockBack(hitEntity, direction, 0f, 0.1f, 12);
            break;
        }
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}