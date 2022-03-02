using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar_Attack1 : SkillEffect
{
    public AnimationClip hitEffectClip;

    public override void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        switch (hitEntity?.gameObject.tag)
        {
            case "Player": // player
            hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.2f});
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.2f});
            StartCoroutine(DamageColor(hitEntity));
            StartCoroutine(KnockBack(hitEntity, direction, 0.1f, 8));
            break;
            case "Enemy": // enemy
            SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
            soundManager.SoundStart("HitSound2", transform);
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.4f});
            hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{0.4f});
            StartCoroutine(DamageColor(hitEntity));
            StartCoroutine(KnockBack(hitEntity, direction, 0.2f, 8));
            break;
        }
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        if (hitEntity != null)
        {
            HitEffect hitEffect = poolManager.GetObject("HitEffect").GetComponent<HitEffect>();
            hitEffect.transform.position = hitEntity.transform.position + Vector3.up * 0.5f;
            hitEffect.Play(hitEffectClip);
            BloodEffect bloodEffect = poolManager.GetObject("BloodEffect").GetComponent<BloodEffect>();
            bloodEffect.transform.position = hitEntity.transform.position;
            bloodEffect.Play();
        }
    }

    IEnumerator KnockBack(Entity hitEntity, Vector2 direction, float time, float power)
    {
        if (hitEntity != null)
        {
            Vector3 dir = new Vector3(direction.x, 0, direction.y).normalized;
            float waitTime = 0;
            while (true)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= time)
                {
                    break;
                }
                hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, power });
                yield return null;
            }
            waitTime = 0;

            while (true)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= 0.3f)
                {
                    break;
                }
                hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, Mathf.Lerp(power, 0, waitTime / 0.3f) });
                yield return null;
            }
            hitEntity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
        }
    }
    IEnumerator DamageColor(Entity hitEntity)
    {
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.white });
        yield return new WaitForSeconds(0.1f);
        hitEntity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.black });
    }
}
