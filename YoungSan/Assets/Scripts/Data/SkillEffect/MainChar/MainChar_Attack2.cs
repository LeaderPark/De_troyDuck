using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack2 : SkillEffect
{
    public override void ShowSkillEffect(Entity attackEntity, Entity hitEntity)
    {
        hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.1f});
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.1f});
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("Lock", new object[]{0.1f});
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[]{(hitEntity.transform.position - attackEntity.transform.position).normalized, 1});
        StartCoroutine(DamageColor(hitEntity?.GetComponent<SpriteRenderer>()));
    }

    IEnumerator DamageColor(SpriteRenderer sr)
    {
        if (sr != null)
        {
            sr.color -= (Color.white - Color.red) / 3;
            yield return new WaitForSeconds(0.1f);
            sr.color += (Color.white - Color.red) / 3;
        }
    }
}