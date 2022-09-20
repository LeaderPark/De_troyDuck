using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAttack : MonoBehaviour
{
    public HitBox[] hitBoxes;
    public Samulnori samulnori;

    public SkillData[] skillDatas;

    Dictionary<Entity, SkillData> skills;

    public void Play()
    {
        skills = new Dictionary<Entity, SkillData>();
        for (int count = 0; count < 4; count++)
        {
            for (int index = 0; index < 4; index++)
            {
                if (skillDatas[count].skillSet.entity == samulnori.samulEntities[index])
                {
                    skills[samulnori.samulEntities[index]] = skillDatas[count];
                }
            }
        }
    }

    void Update()
    {
        for (int index = 0; index < samulnori.samulEntities.Count; index++)
        {
            hitBoxes[index].skillData = skills[samulnori.samulEntities[index]];
            hitBoxes[index].transform.position = samulnori.samulEntities[index].transform.position;
        }
    }

    public void DeActiveAll()
    {
        for (int index = 0; index < hitBoxes.Length; index++)
        {
            hitBoxes[index].gameObject.SetActive(false);
        }
    }

    public void Active(int index)
    {
        hitBoxes[index].gameObject.SetActive(true);
    }

    public void DeActive(int index)
    {
        hitBoxes[index].gameObject.SetActive(false);
    }
}
