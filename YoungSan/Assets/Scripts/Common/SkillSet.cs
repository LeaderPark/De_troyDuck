using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SkillSet : MonoBehaviour
{
    public SkillData[] skillDatas;
    public float[] skillCoolTimes {get; set;}
    private bool[] skillCools;

    void Awake()
    {
        skillCools = new bool[skillDatas.Length];
        skillCoolTimes = new float[skillDatas.Length];
        foreach (var item in skillDatas)
        {
            item.gameObject.SetActive(false);
            item.entity = GetComponentInParent<Entity>();
        }
    }

    public void StopSkill()
    {
        foreach (var item in skillDatas)
        {
            item.gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }

    public void ActiveSkill(int index, Vector2 direction, bool isRight, System.Action action)
    {
        if (skillDatas.Length > index)
        {
            if (skillCools[index]) return;
            CoolDown(index);
            action();
            SkillData data = skillDatas[index];
            data.direction = direction;
            StartCoroutine(CheckActiveTime(data, isRight));
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
