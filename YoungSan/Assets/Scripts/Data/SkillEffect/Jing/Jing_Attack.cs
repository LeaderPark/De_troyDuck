using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jing_Attack : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = hitEntity.transform.position - attackEntity.transform.position;
        KnockBack(hitEntity, new Vector2(dir.x, dir.z), 0f, 0.15f, attackEntity.clone.GetStat(StatCategory.Speed) * 1.5f);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = hitEntity.transform.position - attackEntity.transform.position;
        KnockBack(hitEntity, new Vector2(dir.x, dir.z), 0f, 0.15f, attackEntity.clone.GetStat(StatCategory.Speed) * 1.5f);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}