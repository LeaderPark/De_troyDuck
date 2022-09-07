using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class SkillDataCategory
{
    public EventCategory category;
    public SkillData[] skillDatas;
}

public class SkillSet : MonoBehaviour
{
    public SkillDataCategory[] skillDataCategories;
    public Dictionary<EventCategory, SkillData[]> skillDatas;
    public Dictionary<EventCategory, float[]> skillCoolTimes { get; set; }
    public Dictionary<EventCategory, float[]> skillWaitTimes { get; set; }
    public Dictionary<EventCategory, int> skillChargeAmount { get; set; }
    public Dictionary<EventCategory, int> skillStackAmount { get; set; }

    public Entity entity { get; private set; }

    public bool useSkill;
    public bool active;
    public float delayTime;
    public SkillData skillData;
    private SpriteRenderer spriteRenderer;
    public bool running;
    private bool canAttack;

    private List<object> soundDatas;

    void Awake()
    {
        skillDatas = new Dictionary<EventCategory, SkillData[]>();
        skillCoolTimes = new Dictionary<EventCategory, float[]>();
        skillWaitTimes = new Dictionary<EventCategory, float[]>();
        skillChargeAmount = new Dictionary<EventCategory, int>();
        skillStackAmount = new Dictionary<EventCategory, int>();
        soundDatas = new List<object>();
        entity = GetComponentInParent<Entity>();
        spriteRenderer = entity.GetComponent<SpriteRenderer>();

        foreach (SkillDataCategory category in skillDataCategories)
        {
            skillDatas[category.category] = category.skillDatas;
        }
        foreach (var key in skillDatas.Keys)
        {
            skillCoolTimes[key] = new float[skillDatas[key].Length];
            skillWaitTimes[key] = new float[skillDatas[key].Length];
            skillStackAmount[key] = 0;
            skillChargeAmount[key] = 0;
            foreach (var item in skillDatas[key])
            {
                item.skillSet = this;
                foreach (var i in item.hitBoxDatas)
                {
                    i.LeftHitBox.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Reset()
    {
        foreach (var key in skillDatas.Keys)
        {
            for (int i = 0; i < skillCoolTimes[key].Length; i++)
            {
                skillCoolTimes[key][i] = 0;
            }
            for (int i = 0; i < skillWaitTimes[key].Length; i++)
            {
                skillWaitTimes[key][i] = 0;
            }
            skillStackAmount[key] = 0;
            skillChargeAmount[key] = 0;
        }
    }

    public void StopSkill()
    {
        StopAllCoroutines();
        if (!entity.CompareTag("Player"))
        {
            entity.GetComponentInChildren<Silhouette>()?.GetComponent<SpriteRenderer>().material.SetColor("_TingleColor", Color.black);
            spriteRenderer?.material.SetColor("_TingleColor", Color.black);
        }
        active = false;
        if (skillData != null)
        {
            if (skillData.hitBoxDatas.Length > skillData.targetIndex)
            {
                skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.transform.parent.gameObject.SetActive(false);
                skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.gameObject.SetActive(false);
                skillData.hitBoxDatas[skillData.targetIndex].RightHitBox?.gameObject.SetActive(false);
            }
        }
        foreach (var item in soundDatas)
        {
            SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
            soundManager.SoundStop(item);
        }
        soundDatas.Clear();
        useSkill = false;
        running = false;
        canAttack = true;

        EntityEvent entityEvent = entity.GetComponent<EntityEvent>();
        entityEvent.dontmove = false;
        entityEvent.reservate = false;

        entityEvent.CancelSkillEvent();

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        gameManager.StopAfterImage(entity);
    }

    public void ActiveSkill(EventCategory category, int index, Vector2 direction, bool isRight, System.Action action)
    {
        if (!skillDatas.ContainsKey(category))
        {
            return;
        }
        if (!(canAttack && (!useSkill || !running)))
        {
            return;
        }
        if (skillDatas[category].Length <= index)
        {
            return;
        }
        if (skillCoolTimes[category][index] > 0)
        {
            return;
        }
        if ((skillDatas[category][index].waitTime > 0 && skillWaitTimes[category][index] > 0) || skillDatas[category][index].waitTime == 0)
        {
            skillWaitTimes[category][index] = 0;
            index = (index + 1) % skillDatas[category].Length;
            skillStackAmount[category] = (skillStackAmount[category] + 1) % skillDatas[category].Length;
        }

        int useStamina = skillDatas[category][index].CalculateUseStamina();
        if (useStamina > entity.clone.GetStat(StatCategory.Stamina))
        {
            return;
        }
        StopSkill();
        running = true;
        bool isPlayer = false;
        if (entity.CompareTag("Player"))
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            gameManager.AfterImage(entity, skillDatas[category][index].skill.length);
            isPlayer = true;
        }
        else
        {
            StartCoroutine(EnemyTingle(delayTime));
            isPlayer = false;
        }
        SetCoolDown(category, index, skillDatas[category][index].coolTime);
        SetWaitTime(category, index, skillDatas[category][index].waitTime);
        entity.clone.SubStat(StatCategory.Stamina, useStamina);

        StartCoroutine(SetActiveSkill(category, index, direction, isRight, action, isPlayer));
    }

    IEnumerator SetActiveSkill(EventCategory category, int index, Vector2 direction, bool isRight, System.Action action, bool isPlayer)
    {
        if (!isPlayer)
        {
            if (entity.gameObject.CompareTag("Boss"))
            {
                SuperArmour superArmour = entity.entityStatusAilment.GetEntityStatus(typeof(SuperArmour)) as SuperArmour;

                superArmour.SetData(entity);
                superArmour.ActivateForTime(1);
            }
            yield return new WaitForSeconds(delayTime);
        }
        action();
        SkillData data = skillDatas[category][index];
        data.direction = direction;
        skillData = data;

        if (skillData.hitBoxDatas.Length > skillData.targetIndex)
        {
            skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.ClearTargetSet();
            skillData.hitBoxDatas[skillData.targetIndex].RightHitBox?.ClearTargetSet();
        }

        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(SkillEventTrigger)).Invoke(new object[] { entity, data });
        StartCoroutine(attackSound(data));
        StartCoroutine(CheckRunning(data));
    }

    IEnumerator EnemyTingle(float delayTime)
    {
        float time = delayTime / 2;
        float timeStack = 0;

        SpriteRenderer sr = spriteRenderer;
        SpriteRenderer silhouetteSr = entity.GetComponentInChildren<Silhouette>().GetComponent<SpriteRenderer>();

        const float target = 0.6f;

        while (timeStack < time)
        {
            timeStack += Time.deltaTime;
            Color tingle = Color.black;
            tingle.r = target * timeStack / time;
            tingle.g = target * timeStack / time;
            tingle.b = target * timeStack / time;
            sr.material.SetColor("_TingleColor", tingle);
            silhouetteSr.material.SetColor("_TingleColor", tingle);
            yield return null;
        }
        timeStack = 0;
        sr.material.SetColor("_TingleColor", new Color(target, target, target));
        silhouetteSr.material.SetColor("_TingleColor", new Color(target, target, target));
        while (timeStack < time)
        {
            timeStack += Time.deltaTime;
            Color tingle = Color.white;
            tingle.r = target - target * timeStack / time;
            tingle.g = target - target * timeStack / time;
            tingle.b = target - target * timeStack / time;
            sr.material.SetColor("_TingleColor", tingle);
            silhouetteSr.material.SetColor("_TingleColor", tingle);
            yield return null;
        }
        sr.material.SetColor("_TingleColor", Color.black);
        silhouetteSr.material.SetColor("_TingleColor", Color.black);

    }

    IEnumerator CheckRunning(SkillData data)
    {
        yield return new WaitForSeconds(data.skill.length);
        running = false;
        StopSkill();
    }

    IEnumerator attackSound(SkillData data)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        for (int i = 0; i < data.soundStartTimes.Length; i++)
        {
            if (i == 0)
            {
                yield return new WaitForSeconds(data.soundStartTimes[i]);
            }
            else
            {
                yield return new WaitForSeconds(data.soundStartTimes[i] - data.soundStartTimes[i - 1]);
            }
            if (data.attackSounds.Length > i)
            {
                if (data.attackSounds[i] != null)
                {
                    soundDatas.Add(soundManager.SoundStart(data.attackSounds[i].name, transform));
                }
            }
        }
    }

    private void SetCoolDown(EventCategory category, int index, float time)
    {
        skillCoolTimes[category][index] = time;

        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uiManager.skillinterface.CoolDown(category, index);
    }

    public void SetWaitTime(EventCategory category, int index, float time)
    {
        skillWaitTimes[category][index] = time;
    }

    public void SetChargeAmount(EventCategory category, int value)
    {
        skillChargeAmount[category] = value;
    }

    private void Update()
    {
        if (!canAttack && (useSkill == running)) canAttack = true;
        if (active)
        {
            if (skillData != null)
            {
                if (skillData.hitBoxDatas.Length > skillData.targetIndex)
                {
                    skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.transform.parent.gameObject.SetActive(true);
                    skillData.ActiveHitBox(!spriteRenderer.flipX);
                }
            }
        }
        else
        {
            if (skillData != null)
            {
                if (skillData.hitBoxDatas.Length > skillData.targetIndex)
                {
                    if (skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox != null)
                    {
                        if (skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox.transform.parent.gameObject.activeSelf)
                        {
                            skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.transform.parent.gameObject.SetActive(false);
                            skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.gameObject.SetActive(false);
                            skillData.hitBoxDatas[skillData.targetIndex].RightHitBox?.gameObject.SetActive(false);

                            skillData.hitBoxDatas[skillData.targetIndex].LeftHitBox?.ClearTargetSet();
                            skillData.hitBoxDatas[skillData.targetIndex].RightHitBox?.ClearTargetSet();
                        }
                    }
                }
            }
        }
        foreach (var key in skillDatas.Keys)
        {
            for (int i = 0; i < skillCoolTimes[key].Length; i++)
            {
                if (skillCoolTimes[key][i] > 0)
                {
                    skillCoolTimes[key][i] -= Time.deltaTime;
                    if (skillCoolTimes[key][i] <= 0)
                    {
                        skillCoolTimes[key][i] = 0;
                    }
                }
            }
            for (int i = 0; i < skillWaitTimes[key].Length; i++)
            {
                if (skillWaitTimes[key][i] > 0)
                {
                    skillWaitTimes[key][i] -= Time.deltaTime;
                    if (skillWaitTimes[key][i] <= 0)
                    {
                        skillStackAmount[key] = 0;
                        skillWaitTimes[key][i] = 0;
                    }
                }
            }
        }
    }
}
