using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buk_Skill2 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = (hitEntity.transform.position - hitPoint).normalized * 6;
        dir.y = 2;
        Airbone(hitEntity, 0f, dir);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = (hitEntity.transform.position - hitPoint).normalized * 6;
        dir.y = 2;
        Airbone(hitEntity, 0f, dir);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}
