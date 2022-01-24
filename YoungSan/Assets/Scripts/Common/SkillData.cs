using System.Collections;
using System.Collections.Generic;
using NCalc;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public string skillName;
    public HitBox[] LeftHitBox;
    public HitBox[] RightHitBox;

    public Entity entity {get; set;}

    public float startTime;
    public float time;

    public string skillDamageForm;

    public float coolTime;

    public SkillEffect skillEffect;

    public int CalculateSkillDamage()
    {
        string temp = skillDamageForm;
        foreach (var item in System.Enum.GetNames(typeof(StatCategory)))
        {
            temp = temp.Replace("{" + item + "}", entity.clone.GetStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
            temp = temp.Replace("{Max" + item + "}", entity.clone.GetMaxStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
        }
        
        Expression ex = new Expression(temp);
        return (int)ex.Evaluate();
    }

    void Awake()
    {
        foreach (var item in LeftHitBox)
        {
            item.skillData = this;
        }
        foreach (var item in RightHitBox)
        {
            item.skillData = this;
        }
        
    }

    public void ActiveHitBox(bool isRight)
    {
        if (isRight)
        {
            foreach (var item in RightHitBox)
            {
                item.gameObject.SetActive(true);
            }
            foreach (var item in LeftHitBox)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in LeftHitBox)
            {
                item.gameObject.SetActive(true);
            }
            foreach (var item in RightHitBox)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

}
