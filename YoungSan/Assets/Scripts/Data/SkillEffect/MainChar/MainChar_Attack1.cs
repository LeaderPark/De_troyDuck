using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    public override void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        switch (hitEntity?.gameObject.layer)
        {
            case 6: // player
            hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.2f});
            StartCoroutine(DamageColor(hitEntity));
            if (hitEntity != null)
            {
                Vector3 dir = new Vector3(direction.x, 0, direction.y);
                StartCoroutine(KnockBack(hitEntity, (dir).normalized, 0.1f, 8));
            }
            break;
            case 7: // enemy
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.4f});
            hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.4f});
            hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("Lock", new object[]{0.4f});
            hitEntity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("Lock", new object[]{0.4f});
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});
            StartCoroutine(DamageColor(hitEntity));
            if (hitEntity != null)
            {
                Vector3 dir = new Vector3(direction.x, 0, direction.y);
                StartCoroutine(KnockBack(hitEntity, (dir).normalized, 0.2f, 8));
            }
            break;
        }
    }

    IEnumerator KnockBack(Entity hitEntity, Vector3 dir, float time, float power)
    {
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, power });
        yield return new WaitForSeconds(time);
        hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
    }
    IEnumerator DamageColor(Entity hitEntity)
    {
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.white });
        yield return new WaitForSeconds(0.1f);
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.black });
    }
}
