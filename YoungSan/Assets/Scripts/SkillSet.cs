using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet : MonoBehaviour
{
    public SkillData[] skillDatas;

    void Awake()
    {
        foreach (var item in skillDatas)
        {
            item.gameObject.SetActive(false);
            item.entity = GetComponentInParent<Entity>();
        }
    }

    public void ActiveSkill(int index, bool isRight)
    {
        if (skillDatas.Length > index)
        {
            SkillData data = skillDatas[index];
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
}
