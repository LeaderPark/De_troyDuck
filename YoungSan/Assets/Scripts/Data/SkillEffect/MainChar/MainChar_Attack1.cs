using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    public override void ShowSkillEffect(Entity attackEntity, Entity hitEntity)
    {
        hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.1f});
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.1f});
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("Lock", new object[]{0.1f});
     // hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[]{(hitEntity.transform.position - attackEntity.transform.position).normalized, 1});
        StartCoroutine(DamageColor(hitEntity?.GetComponent<SpriteRenderer>()));
        if (hitEntity != null)
        {
            Vector3 dir = hitEntity.transform.position - attackEntity.transform.position;
            dir.y = 0;
            StartCoroutine(KnockBack(hitEntity, (dir).normalized, 0.1f));
        }
    }

    IEnumerator KnockBack(Entity hitEntity, Vector3 dir, float time)
    {
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, 8 });
        yield return new WaitForSeconds(time);
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
    }
    IEnumerator DamageColor(SpriteRenderer sr)
    {
        if (sr != null)
        {
            sr.color -= (Color.white - Color.red) / 5;
            yield return new WaitForSeconds(0.1f);
            sr.color += (Color.white - Color.red) / 5;
        }
    }
}
