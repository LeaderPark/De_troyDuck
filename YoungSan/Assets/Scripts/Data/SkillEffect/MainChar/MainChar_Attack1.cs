using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    public override void ShowSkillEffect(Entity attackEntity, Entity hitEntity)
    {
        hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.2f});
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.2f});
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("Lock", new object[]{0.2f});
        StartCoroutine(DamageColor(hitEntity?.GetComponent<SpriteRenderer>()));
    }

    IEnumerator DamageColor(SpriteRenderer sr)
    {
        if (sr != null)
        {
            sr.color -= (Color.white - Color.red) / 5;
            yield return new WaitForSeconds(0.5f);
            sr.color += (Color.white - Color.red) / 5;
        }
    }
}
