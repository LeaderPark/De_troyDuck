using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{
    public AnimationClip hitEffectClip;
    public AudioClip hitSoundClip;

    public void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector2 direction)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        if(hitSoundClip!=null)
        soundManager.SoundStart(hitSoundClip.name, transform);
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
        hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});

        switch (hitEntity?.gameObject.tag)
        {
            case "Player": // player
            ShowPlayerEffect(attackEntity, hitEntity, direction);
            break;
            case "Enemy": // enemy
            ShowEnemyEffect(attackEntity, hitEntity, direction);
            break;
        }
    }

    protected abstract void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction);
    protected abstract void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction);

    protected void Stiff(Entity entity, float time)
    {
        entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{time});
        entity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{time});
        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("Lock", new object[]{time});
    }

    protected void ChangeColor(Entity entity, Color color, float time)
    {
        StartCoroutine(ChangeColorProcess(entity, color, time));
    }

    private IEnumerator ChangeColorProcess(Entity entity, Color color, float time)
    {
        entity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { color });
        yield return new WaitForSeconds(time);
        entity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.black });
    }

    protected void KnockBack(Entity entity, Vector2 direction, float time, float power)
    {
        StartCoroutine(KnockBackProcess(entity, direction, time, power));
    }

    private IEnumerator KnockBackProcess(Entity entity, Vector2 direction, float time, float power)
    {
        if (entity != null)
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
                entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, power });
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
                entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { dir, Mathf.Lerp(power, 0, waitTime / 0.3f) });
                yield return null;
            }
            entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
        }
    }
}
