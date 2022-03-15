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
    private Dictionary<EventCategory,bool[]> skillCools;

    public Entity entity {get; private set;}

    void Awake()
    {
        skillDatas = new Dictionary<EventCategory, SkillData[]>();
        skillCools = new Dictionary<EventCategory, bool[]>();
        skillCoolTimes = new Dictionary<EventCategory, float[]>();
        entity = GetComponentInParent<Entity>();

        foreach (SkillDataCategory category in skillDataCategories)
        {
            skillDatas[category.category] = category.skillDatas;
        }
        foreach (var key in skillDatas.Keys)
        {
            skillCools[key] = new bool[skillDatas[key].Length];
            skillCoolTimes[key] = new float[skillDatas[key].Length];
            foreach (var item in skillDatas[key])
            {
                item.gameObject.SetActive(false);
                item.skillSet = this;
            }
        }
    }

    public void StopSkill()
    {
        StopAllCoroutines();
        foreach (var key in skillDatas.Keys)
        {
            foreach (var item in skillDatas[key])
            {
                item.gameObject.SetActive(false);
            }
        }
        
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        gameManager.StopAfterImage(entity);
    }

    public void ActiveSkill(EventCategory category, int index, Vector2 direction, bool isRight, System.Action action)
    {
        if (!skillDatas.ContainsKey(category)) return;
        if (skillDatas[category].Length > index)
        {
            if (skillCools[category][index]) return;
            int useStamina = skillDatas[category][index].CalculateUseStamina();
            if (useStamina > entity.clone.GetStat(StatCategory.Stamina)) return;
            CoolDown(category, index);
            StopSkill();
            
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (entity.CompareTag("Player")) gameManager.AfterImage(entity, skillDatas[category][index].startTime + skillDatas[category][index].time);
            entity.clone.SubStat(StatCategory.Stamina, useStamina);
            action();
            SkillData data = skillDatas[category][index];
            data.direction = direction;
            
            EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
            eventManager.GetEventTrigger(typeof(SkillEventTrigger)).Invoke(new object[]{ entity, data });

            StartCoroutine(CheckActiveTime(data, isRight));
            StartCoroutine(attackSound(data));
        }
    }

    IEnumerator CheckActiveTime(SkillData data, bool isRight)
    {
        yield return new WaitForSeconds(data.startTime);
        data.gameObject.SetActive(true);
        data.ActiveHitBox(isRight);
        yield return new WaitForSeconds(data.time);
        data.gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator attackSound(SkillData data)
    {
        SoundManager soundManager = ManagerObject.Instance.GetManager(ManagerType.SoundManager) as SoundManager;
        yield return new WaitForSeconds(data.soundStartTime);
        if(data.attackSound!=null)
        soundManager.SoundStart(data.attackSound.name, transform);
    }

    private void CoolDown(EventCategory category, int index)
    {
        skillCools[category][index] = true;
        skillCoolTimes[category][index] = skillDatas[category][index].coolTime;

        UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uiManager.skillinterface.CoolDown(category, index);
    }

    private void Update()
    {
        foreach (var key in skillCoolTimes.Keys)
        {
            for (int i = 0; i < skillCoolTimes[key].Length; i++)
            {
                if (skillCools[key][i])
                {
                    skillCoolTimes[key][i] -= Time.deltaTime;
                    if (skillCoolTimes[key][i] <= 0)
                    {
                        skillCools[key][i] = false;
                    }
                }
            }
        }
    }
}
