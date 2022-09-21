using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{
    public AnimationClip[] hitEffectClips;
    public AudioClip[] hitSoundClips;

    public void ShowSkillEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index)
    {
        if (hitEntity == null)
        {
            ShowWallEffect(attackEntity, direction, index);
            return;
        }
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        if (hitSoundClips[index] != null)
            soundManager.SoundStart(hitSoundClips[index].name, transform);
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        if (hitEntity != null)
        {
            Effect hitEffect = poolManager.GetObject("HitEffect").GetComponent<Effect>();
            hitEffect.transform.position = hitEntity.transform.position + Vector3.up * 0.5f;
            hitEffect.Play(hitEffectClips[index]);
            BloodEffect bloodEffect = poolManager.GetObject("BloodEffect").GetComponent<BloodEffect>();
            bloodEffect.transform.position = hitEntity.transform.position;
            bloodEffect.Play();
        }

        switch (hitEntity?.gameObject.tag)
        {
            case "Player": // player
                hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Hit" });
                ShowPlayerEffect(attackEntity, hitEntity, hitPoint, direction, index);
                break;
            case "Enemy": // enemy
                hitEntity?.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Hit" });
                ShowEnemyEffect(attackEntity, hitEntity, hitPoint, direction, index);
                break;
            case "Boss": // enemy
                ShowEnemyEffect(attackEntity, hitEntity, hitPoint, direction, index);
                break;
        }
    }

    protected abstract void ShowPlayerEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index);
    protected abstract void ShowEnemyEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index);
    protected abstract void ShowBossEffect(Entity attackEntity, Entity hitEntity, Vector3 hitPoint, Vector2 direction, int index);
    protected virtual void ShowWallEffect(Entity attackEntity, Vector2 direction, int index) { }

    protected void Stiff(Entity entity, float time)
    {
        Fainting fainting = entity.entityStatusAilment.GetEntityStatus(typeof(Fainting)) as Fainting;

        fainting.SetData(entity);
        fainting.ActivateForTime(time);
    }

    protected void TickDamage(TickAilment tickAilment, Entity sourceEntity, Entity targetEntity, float delay, float time, string tickDamageForm)
    {
        switch (tickAilment)
        {
            case TickAilment.Igniting:

                Igniting igniting = targetEntity.entityStatusAilment.GetEntityStatus(typeof(Igniting)) as Igniting;

                igniting.SetData(sourceEntity, targetEntity, delay, time, tickDamageForm);
                igniting.ActivateForTime(time);

                break;
            case TickAilment.Poisoning:

                Poisoning poisoning = targetEntity.entityStatusAilment.GetEntityStatus(typeof(Poisoning)) as Poisoning;

                poisoning.SetData(sourceEntity, targetEntity, delay, time, tickDamageForm);
                poisoning.ActivateForTime(time);

                break;
            case TickAilment.Bleeding:

                Bleeding bleeding = targetEntity.entityStatusAilment.GetEntityStatus(typeof(Bleeding)) as Bleeding;

                bleeding.SetData(sourceEntity, targetEntity, delay, time, tickDamageForm);
                bleeding.ActivateForTime(time);

                break;
        }
    }

    protected void Buff(Entity targetEntity, float startTime, float time, StatCategory category, int value)
    {
        Coroutine routine = this.StartCoroutine(BuffRoutine(targetEntity, startTime, time, category, value));
    }

    private IEnumerator BuffRoutine(Entity targetEntity, float startTime, float time, StatCategory category, int value)
    {
        yield return new WaitForSeconds(startTime);

        targetEntity.extraStat[category] += value;
        targetEntity.clone.SetMaxStat(category, targetEntity.clone.GetMaxStat(category) + value);
        targetEntity.clone.SetStat(category, targetEntity.clone.GetStat(category) + value);

        yield return new WaitForSeconds(time);

        targetEntity.extraStat[category] -= value;
        targetEntity.clone.SetStat(category, targetEntity.clone.GetStat(category) - value);
        targetEntity.clone.SetMaxStat(category, targetEntity.clone.GetMaxStat(category) - value);
    }

    protected void Airbone(Entity targetEntity, float startTime, Vector3 power)
    {
        Coroutine routine = this.StartCoroutine(AirboneRoutine(targetEntity, startTime, power));
    }

    private IEnumerator AirboneRoutine(Entity targetEntity, float startTime, Vector3 power)
    {
        yield return new WaitForSeconds(startTime);

        Airbone airbone = targetEntity.entityStatusAilment.GetEntityStatus(typeof(Airbone)) as Airbone;

        targetEntity.GetComponentInChildren<SkillSet>().StopSkill();
        airbone.SetData(targetEntity, power);
        airbone.Activate();
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
            targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("LockTime", new object[] { Time.deltaTime });
            targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { (sourceEntity.transform.position - targetEntity.transform.position).normalized, speed });
            yield return null;
        }
        targetEntity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
    }

    public void ChangeColor(Entity entity, Color color, float startTime, float time)
    {
        StartCoroutine(ChangeColorProcess(entity, color, startTime, time));
    }

    private IEnumerator ChangeColorProcess(Entity entity, Color color, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);
        entity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { color });
        yield return new WaitForSeconds(time);
        entity?.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetColor", new object[] { Color.black });
    }

    protected void KnockBack(Entity entity, Vector2 direction, float startTime, float time, float power)
    {
        StartCoroutine(KnockBackProcess(entity, direction, startTime, time, power));
    }

    private IEnumerator KnockBackProcess(Entity entity, Vector2 direction, float startTime, float time, float power)
    {
        if (entity != null)
        {
            yield return new WaitForSeconds(startTime);
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
