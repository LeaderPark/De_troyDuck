using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    protected override void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.2f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        // KnockBack(hitEntity, direction, 0f, 0.1f, 8);
        Airbone(hitEntity, 0f, new Vector3(direction.x, 0, direction.y).normalized * 3 + new Vector3(0, 6, 0));
    }

    protected override void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        Stiff(hitEntity, 0.4f);
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);
        // KnockBack(hitEntity, direction, 0f, 0.2f, 8);

        TickDamage(TickAilment.Poisoning, attackEntity, hitEntity, 1, 5, "20");
        Airbone(hitEntity, 0f, new Vector3(direction.x, 0, direction.y).normalized * 3 + new Vector3(0, 6, 0));
    }

    protected override void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        ChangeColor(hitEntity, Color.red, 0f, 0.1f);

        Airbone(hitEntity, 0f, new Vector3(direction.x, 0, direction.y).normalized * 3 + new Vector3(0, 6, 0));
    }
}
