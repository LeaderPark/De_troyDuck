using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvent : MonoBehaviour
{
    [HideInInspector]
    public Entity entity;
    [HideInInspector]
    public SkillSet skillSet;
    public List<(bool, Coroutine)> coroutines;

    public void CallEvent(EventCategory eventCategory, float inputX, float inputY, bool direction, Vector3 position)
    {
        switch (eventCategory)
        {
            case EventCategory.Move:
                CallMove(inputX, inputY, direction, position);
                break;
            case EventCategory.DefaultAttack:
                CallDefaultAttack(inputX, inputY, direction, position);
                break;
            case EventCategory.Skill1:
                CallSkill1(inputX, inputY, direction, position);
                break;
            case EventCategory.Skill2:
                CallSkill2(inputX, inputY, direction, position);
                break;
            case EventCategory.Skill3:
                CallSkill3(inputX, inputY, direction, position);
                break;
        }
    }

    protected virtual void Awake()
    {
        dontmove = false;
        maxAttackStack = new Dictionary<EventCategory, int>();
        attackProcess = new Dictionary<EventCategory, AttackProcess[]>();
        entity = GetComponent<Entity>();
        skillSet = entity.GetComponentInChildren<SkillSet>();
        coroutines = new List<(bool, Coroutine)>();
    }

    private void Start()
    {
        foreach (EventCategory category in maxAttackStack.Keys)
        {
            StartCoroutine(AttackEndCheck(category));
        }
    }

    public bool dontmove;
    public bool reservate;
    protected void CallMove(float inputX, float inputY, bool direction, Vector3 position)
    {
        if (skillSet.running && skillSet.skillData)
        {
            dontmove = !skillSet.skillData.canmove;
        }
        if (!dontmove)
        {
            if (skillSet.running && skillSet.skillData)
            {
                if (skillSet.skillData.canrotate)
                {
                    if (!(entity.GetProcessor(typeof(Processor.Sprite)) as Processor.Sprite).locking)
                    {
                        entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[] { direction });
                    }
                }
            }
            else
            {
                if (!(entity.GetProcessor(typeof(Processor.Sprite)) as Processor.Sprite).locking)
                {
                    entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[] { direction });
                }
            }
            if (!skillSet.running)
            {
                if (inputX == 0 && inputY == 0)
                {
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { "Idle", false });
                }
                else
                {
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[] { "Move", false });
                }
            }

            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[] { new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) });
            entity.GetProcessor(typeof(Processor.Collision))?.AddCommand("SetCollider", new object[] { GetComponent<SpriteRenderer>().sprite });
        }

    }

    public delegate void AttackProcess(float inputX, float inputY, Vector3 position, SkillData skillData);

    protected Dictionary<EventCategory, int> maxAttackStack;
    protected Dictionary<EventCategory, AttackProcess[]> attackProcess;
    protected void CallDefaultAttack(float inputX, float inputY, bool direction, Vector3 position)
    {
        AttackSkillEvent(EventCategory.DefaultAttack, inputX, inputY, direction, position);
    }
    protected void CallSkill1(float inputX, float inputY, bool direction, Vector3 position)
    {
        AttackSkillEvent(EventCategory.Skill1, inputX, inputY, direction, position);
    }
    protected void CallSkill2(float inputX, float inputY, bool direction, Vector3 position)
    {
        AttackSkillEvent(EventCategory.Skill2, inputX, inputY, direction, position);
    }
    protected void CallSkill3(float inputX, float inputY, bool direction, Vector3 position)
    {
        AttackSkillEvent(EventCategory.Skill3, inputX, inputY, direction, position);
    }


    public void CancelSkillEvent()
    {
        if (coroutines.Count > 0)
        {
            for (int i = 0; i < coroutines.Count; i++)
            {
                if (!coroutines[i].Item1)
                {
                    StopCoroutine(coroutines[i].Item2);
                }
            }
            coroutines.Clear();
        }
    }


    private void AttackSkillEvent(EventCategory category, float inputX, float inputY, bool direction, Vector3 position)
    {
        if (!skillSet.skillStackAmount.ContainsKey(category)) return;

        entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{category, skillSet.skillStackAmount[category], new Vector2(inputX, inputY), direction, (System.Action)(() =>
        {
            entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{skillSet.skillDatas[category][skillSet.skillStackAmount[category]].skill.name, true});
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
            attackProcess[category][skillSet.skillStackAmount[category]]?.Invoke(inputX, inputY, position, skillSet.skillDatas[category][skillSet.skillStackAmount[category]]);
            reservate = true;
        })});
    }

    protected void Dash(float inputX, float inputY, float speed, float startTime, float time)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(DashRoutine(idx, inputX, inputY, speed, startTime, time));
        coroutines.Add((false, routine));
    }

    private IEnumerator DashRoutine(int idx, float inputX, float inputY, float speed, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[] { new Vector3(inputX, 0, inputY).normalized, speed });
        yield return new WaitForSeconds(time);
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[] { Vector3.zero, 0 });
    }

    protected void Flash(Vector3 position, float range, float startTime)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(FlashRoutine(idx, position, range, startTime));
        coroutines.Add((false, routine));
    }

    private IEnumerator FlashRoutine(int idx, Vector3 position, float range, float startTime)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Vector3 pos = position;
        if ((pos - entity.transform.position).sqrMagnitude > range * range)
        {
            pos = (pos - entity.transform.position).normalized * range + entity.transform.position;
        }
        entity.transform.position = pos;
    }

    protected void Projectile(float inputX, float inputY, string objectName, SkillData skillData, Vector3 position, bool setDir, float startTime)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(ProjectileRoutine(idx, inputX, inputY, objectName, skillData, position, setDir, startTime));
        coroutines.Add((false, routine));
    }

    private IEnumerator ProjectileRoutine(int idx, float inputX, float inputY, string objectName, SkillData skillData, Vector3 position, bool setDir, float startTime)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Projectile projectile = poolManager.GetObject(objectName).GetComponent<Projectile>();
        projectile.SetData(position, new Vector3(inputX, 0, inputY).normalized, skillData, setDir);
    }

    protected void Installation(Vector3 position, SkillData skillData, string objectName, float startTime)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(InstallationRoutine(idx, position, skillData, objectName, startTime));
        coroutines.Add((false, routine));
    }

    private IEnumerator InstallationRoutine(int idx, Vector3 position, SkillData skillData, string objectName, float startTime)
    {
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Installation installation = poolManager.GetObject(objectName).GetComponent<Installation>();
        installation.SetData(entity, position, skillData);

    }

    protected void Heal(float startTime, float time, float delay, float rate)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(HealRoutine(idx, startTime, time, delay, rate));
        coroutines.Add((false, routine));
    }

    private IEnumerator HealRoutine(int idx, float startTime, float time, float delay, float rate)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        int healCount = (int)(time / delay);
        float timeStack = 0;
        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

        int count = healCount;
        while (count > 0)
        {
            timeStack += Time.deltaTime;

            if (timeStack >= delay)
            {
                timeStack = 0;

                int healValue = (int)(entity.clone.GetMaxStat(StatCategory.Health) * rate / healCount);
                entity.clone.AddStat(StatCategory.Health, healValue);

                uiManager.damageCountUI.Play(entity.transform.position + Vector3.up * entity.entityData.uiPos * 0.5f, healValue, false, true);

                count--;
            }
            yield return null;
        }
    }

    protected void Buff(float startTime, float time, StatCategory category, int value)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(BuffRoutine(idx, startTime, time, category, value));
        coroutines.Add((false, routine));
    }

    private IEnumerator BuffRoutine(int idx, float startTime, float time, StatCategory category, int value)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);


        int tempMaxStat = entity.clone.GetMaxStat(category);
        int tempStat = entity.clone.GetStat(category);

        entity.extraStat[category] += value;
        entity.clone.SetMaxStat(category, tempMaxStat + value);
        entity.clone.SetStat(category, tempStat + value);

        yield return new WaitForSeconds(time);

        tempMaxStat = entity.clone.GetMaxStat(category);
        tempStat = entity.clone.GetStat(category);

        entity.extraStat[category] -= value;
        entity.clone.SetMaxStat(category, tempMaxStat - value);
        entity.clone.SetStat(category, tempStat - value);
    }

    protected void Defend(float startTime, float time, float rate, int value)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(DefendRoutine(idx, startTime, time, rate, value));
        coroutines.Add((false, routine));
    }

    private IEnumerator DefendRoutine(int idx, float startTime, float time, float rate, int value)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Defending defending = entity.entityStatusAilment.GetEntityStatus(typeof(Defending)) as Defending;

        defending.SetData(rate, value);
        defending.ActivateForTime(time);
    }

    protected void SuperArmour(Entity target, float startTime, float time)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(SuperArmourRoutine(idx, target, startTime, time));
        coroutines.Add((false, routine));
    }

    private IEnumerator SuperArmourRoutine(int idx, Entity target, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        SuperArmour superArmour = target.entityStatusAilment.GetEntityStatus(typeof(SuperArmour)) as SuperArmour;

        superArmour.SetData(target);
        superArmour.ActivateForTime(time);
    }

    protected void Blocking(float startTime, float time)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(BlockingRoutine(idx, startTime, time));
        coroutines.Add((false, routine));
    }

    private IEnumerator BlockingRoutine(int idx, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Blocking blocking = entity.entityStatusAilment.GetEntityStatus(typeof(Blocking)) as Blocking;

        blocking.SetData(entity);
        blocking.ActivateForTime(time);
    }

    protected void Airbone(float startTime, Vector3 power)
    {
        int idx = coroutines.Count;
        Coroutine routine = this.StartCoroutine(AirboneRoutine(idx, startTime, power));
        coroutines.Add((false, routine));
    }

    private IEnumerator AirboneRoutine(int idx, float startTime, Vector3 power)
    {
        yield return new WaitForSeconds(startTime);
        coroutines[idx] = (true, coroutines[idx].Item2);

        Airbone airbone = entity.entityStatusAilment.GetEntityStatus(typeof(Airbone)) as Airbone;

        entity.GetComponentInChildren<SkillSet>().StopSkill();
        airbone.SetData(entity, power);
        airbone.Activate();
    }

    private IEnumerator AttackEndCheck(EventCategory category)
    {
        while (true)
        {
            System.Action<bool, float> end = (System.Action<bool, float>)((bool transition, float time) =>
            {
                if (!transition && time == 0f)
                {
                    reservate = false;
                }
                else if (!transition && time >= 1f)
                {
                    if (reservate) return;
                    dontmove = false;
                }
                else if (transition)
                {
                    reservate = false;
                    dontmove = false;
                }
            });

            if (skillSet.skillDatas.ContainsKey(category))
            {
                foreach (var data in skillSet.skillDatas[category])
                {
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[] { data.skill.name, end });
                }
            }
            yield return null;
        }
    }
}

public enum EventCategory
{
    Move,
    DefaultAttack,
    Skill1,
    Skill2,
    Skill3
}
