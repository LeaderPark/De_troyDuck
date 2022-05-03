using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{
    public AnimationClip[] hitEffectClips;
    public AudioClip[] hitSoundClips;

    public void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        if(hitSoundClips[index] != null)
        soundManager.SoundStart(hitSoundClips[index].name, transform);
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        if (hitEntity != null)
        {
            HitEffect hitEffect = poolManager.GetObject("HitEffect").GetComponent<HitEffect>();
            hitEffect.transform.position = hitEntity.transform.position + Vector3.up * 0.5f;
            hitEffect.Play(hitEffectClips[index]);
            BloodEffect bloodEffect = poolManager.GetObject("BloodEffect").GetComponent<BloodEffect>();
            bloodEffect.transform.position = hitEntity.transform.position;
            bloodEffect.Play();
        }

        switch (hitEntity?.gameObject.tag)
        {
            case "Player": // player
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});
            ShowPlayerEffect(attackEntity, hitEntity, direction, index);
            break;
            case "Enemy": // enemy
            hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Hit"});
            ShowEnemyEffect(attackEntity, hitEntity, direction, index);
            break;
            case "Boss": // enemy
            ShowBossEffect(attackEntity, hitEntity, direction, index);
            break;
        }
    }

    protected abstract void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index);
    protected abstract void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index);
    protected abstract void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector2 direction, int index);

    protected void Stiff(Entity entity, float time)
    {
        entity?.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{time});
        entity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{time});
        entity?.GetProcessor(typeof(Processor.Skill))?.AddCommand("Lock", new object[]{time});
    }

    protected void TickDamage(string tickDamage, Entity sourceEntity, Entity targetEntity, float delay, float time)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        TickDamage tick = poolManager.GetObject(tickDamage).GetComponent<TickDamage>();
        tick.SetData(sourceEntity, targetEntity, delay, time);
    }

    protected void Grab(Entity sourceEntity, Entity targetEntity, float speed, float startTime, float time)
    {
        StartCoroutine(GrabRoutine(sourceEntity, targetEntity, speed, startTime, time));
    }

    private IEnumerator GrabRoutine(Entity sourceEntity, Entity targetEntity, float speed, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);

        float timeStack = 0;
        while (timeStack < time && Vector3.Distance(sourceEntity.transform.position, targetEntity.transform.position) > 1f)
        {
            timeStack += Time.deltaTime;
            targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[]{Time.deltaTime});
            targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[]{(sourceEntity.transform.position - targetEntity.transform.position).normalized, speed});
            yield return null;
        }
        targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[]{Vector3.zero, 0});
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
