using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SkillSet : MonoBehaviour
{
    public SkillData[] skillDatas;
    public float[] skillCoolTimes {get; set;}
    private bool[] skillCools;

    public Entity entity {get; private set;}

    void Awake()
    {
        skillCools = new bool[skillDatas.Length];
        skillCoolTimes = new float[skillDatas.Length];
        entity = GetComponentInParent<Entity>();
        foreach (var item in skillDatas)
        {
            item.gameObject.SetActive(false);
            item.skillSet = this;
        }
    }

    public void StopSkill()
    {
        StopAllCoroutines();
        foreach (var item in skillDatas)
        {
            item.gameObject.SetActive(false);
        }
        
        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        gameManager.StopAfterImage(entity);
    }

    public void ActiveSkill(int index, Vector2 direction, bool isRight, System.Action action)
    {
        if (skillDatas.Length > index)
        {
            if (skillCools[index]) return;
            int useStamina = skillDatas[index].CalculateUseStamina();
            if (useStamina > entity.clone.GetStat(StatCategory.Stamina)) return;
            CoolDown(index);
            StopSkill();
            
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (entity.CompareTag("Player")) gameManager.AfterImage(entity, skillDatas[index].startTime + skillDatas[index].time);
            entity.clone.SubStat(StatCategory.Stamina, useStamina);
            action();
            SkillData data = skillDatas[index];
            data.direction = direction;
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

    private void CoolDown(int index)
    {
        skillCools[index] = true;
        skillCoolTimes[index] = skillDatas[index].coolTime;
    }

    private void Update()
    {
        for (int i = 0; i < skillCoolTimes.Length; i++)
        {
            if (skillCools[i])
            {
                skillCoolTimes[i] -= Time.deltaTime;
                if (skillCoolTimes[i] <= 0)
                {
                    skillCools[i] = false;
                }
            }
        }
    }
}
