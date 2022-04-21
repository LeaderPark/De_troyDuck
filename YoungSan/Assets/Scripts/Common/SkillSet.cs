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
    public Dictionary<EventCategory, float[]> skillCoolTimes {get; set;}
    public Dictionary<EventCategory, float[]> skillWaitTimes {get; set;}
    public Dictionary<EventCategory, int> skillStacks {get; set;}

    public Entity entity {get; private set;}

    public bool useSkill;
    public bool active;
    private SkillData skillData;
    private bool isRight;
    private bool running;
    private bool canAttack;

    void Awake()
    {
        skillDatas = new Dictionary<EventCategory, SkillData[]>();
        skillCoolTimes = new Dictionary<EventCategory, float[]>();
        skillWaitTimes = new Dictionary<EventCategory, float[]>();
        skillStacks = new Dictionary<EventCategory, int>();
        entity = GetComponentInParent<Entity>();

        foreach (SkillDataCategory category in skillDataCategories)
        {
            skillDatas[category.category] = category.skillDatas;
        }
        foreach (var key in skillDatas.Keys)
        {
            skillCoolTimes[key] = new float[skillDatas[key].Length];
            skillWaitTimes[key] = new float[skillDatas[key].Length];
            foreach (var item in skillDatas[key])
            {
                item.gameObject.SetActive(false);
                item.skillSet = this;
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
        }
    }

    public void StopSkill()
    {
        StopAllCoroutines();
        active = false;
        skillData?.gameObject.SetActive(false);
        useSkill = false;
        running = false;
        canAttack = false;
        
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
        int useStamina = skillDatas[category][index].CalculateUseStamina();
        if (useStamina > entity.clone.GetStat(StatCategory.Stamina)) 
        {
            return;
        }
        StopSkill();
        running = true;
        
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        if (entity.CompareTag("Player"))
        {
            gameManager.AfterImage(entity, skillDatas[category][index].skill.length);
        }
        SetCoolDown(category, index, skillDatas[category][index].coolTime);
        SetWaitTime(category, index, skillDatas[category][index].waitTime);
        entity.clone.SubStat(StatCategory.Stamina, useStamina);
        action();
        SkillData data = skillDatas[category][index];
        data.direction = direction;
        skillData = data;
        this.isRight = isRight;
        
        EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
        eventManager.GetEventTrigger(typeof(SkillEventTrigger)).Invoke(new object[]{ entity, data });
        StartCoroutine(attackSound(data));
        StartCoroutine(CheckRunning(data));
    }

    IEnumerator CheckRunning(SkillData data)
    {
        yield return new WaitForSeconds(data.skill.length);
        running = false;
    }

    IEnumerator attackSound(SkillData data)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        yield return new WaitForSeconds(data.soundStartTime);
        if(data.attackSound!=null)
        soundManager.SoundStart(data.attackSound.name, transform);
    }

    private void SetCoolDown(EventCategory category, int index, float time)
    {
        skillCoolTimes[category][index] = time;

        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uiManager.skillinterface.CoolDown(category, index);
    }

    private void SetWaitTime(EventCategory category, int index, float time)
    {
        skillWaitTimes[category][index] = time;
    }

    private void Update()
    {
        if (!canAttack && (useSkill == running)) canAttack = true; 
        if (active)
        {
            skillData.gameObject.SetActive(true);
            skillData.ActiveHitBox(isRight);
        }
        else
        {
            skillData?.gameObject.SetActive(false);
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
                        skillWaitTimes[key][i] = 0;
                    }
                }
            }
        }
    }
}
