using System.Collections;
using System.Collections.Generic;
using NCalc;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public AnimationClip skill;
    public HitBox[] LeftHitBox;
    public HitBox[] RightHitBox;

    public SkillSet skillSet {get; set;}
    public float soundStartTime;

    public string skillDamageForm;

    public float coolTime;
    public float waitTime;
    public string useStaminaForm;

    public SkillEffect skillEffect;
    public AudioClip attackSound;
    public Vector2 direction;

    public int CalculateSkillDamage()
    {
        string temp = skillDamageForm;
        foreach (var item in System.Enum.GetNames(typeof(StatCategory)))
        {
            temp = temp.Replace("{" + item + "}", skillSet.entity.clone.GetStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
            temp = temp.Replace("{Max" + item + "}", skillSet.entity.clone.GetMaxStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
        }
        
        Expression ex = new Expression(temp);
        return (int)ex.Evaluate();
    }

    public int CalculateUseStamina()
    {
        string temp = useStaminaForm;
        foreach (var item in System.Enum.GetNames(typeof(StatCategory)))
        {
            temp = temp.Replace("{" + item + "}", skillSet.entity.clone.GetStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
            temp = temp.Replace("{Max" + item + "}", skillSet.entity.clone.GetMaxStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
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
