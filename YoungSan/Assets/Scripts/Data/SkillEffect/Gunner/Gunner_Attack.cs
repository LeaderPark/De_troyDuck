using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_Attack : SkillEffect
{

    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        KnockBack(hitEntity, hitEntity.transform.position - hitPoint, 0f, 0.2f, 8);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        KnockBack(hitEntity, hitEntity.transform.position - hitPoint, 0f, 0.2f, 8);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}