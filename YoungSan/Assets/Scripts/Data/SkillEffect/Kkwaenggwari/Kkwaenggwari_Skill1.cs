using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kkwaenggwari_Skill1 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Buff(hitEntity, 0f, 10f, StatCategory.Attack, -hitEntity.clone.GetStat(StatCategory.Attack) / 2);
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        Buff(hitEntity, 0f, 10f, StatCategory.Attack, -hitEntity.clone.GetStat(StatCategory.Attack) / 2);
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
    }
}
