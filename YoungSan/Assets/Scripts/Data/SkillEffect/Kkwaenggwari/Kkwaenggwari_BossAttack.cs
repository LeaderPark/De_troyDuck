using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kkwaenggwari_BossAttack : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = hitEntity.transform.position - attackEntity.transform.position;
        KnockBack(hitEntity, new Vector2(dir.x, dir.z), 0f, 0.2f, attackEntity.clone.GetStat(StatCategory.Speed));
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Vector3 dir = hitEntity.transform.position - attackEntity.transform.position;
        KnockBack(hitEntity, new Vector2(dir.x, dir.z), 0f, 0.2f, attackEntity.clone.GetStat(StatCategory.Speed));
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}
