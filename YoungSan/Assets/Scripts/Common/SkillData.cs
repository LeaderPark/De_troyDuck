using System.Collections;
using System.Collections.Generic;
using NCalc;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public AnimationClip skill;
    public int targetIndex;

    public HitBoxData[] hitBoxDatas;

    public SkillSet skillSet { get; set; }
    public float[] soundStartTimes;

    public string[] skillDamageForms;

    public bool targeting;
    public bool canmove;
    public bool canrotate;
    public float coolTime;
    public float waitTime;
    public string useStaminaForm;

    public SkillEffect skillEffect;
    public AudioClip[] attackSounds;
    public Vector2 direction;
    public Entity target;

    public int CalculateSkillDamage()
    {
        string temp = skillDamageForms[targetIndex];
        foreach (var item in System.Enum.GetNames(typeof(StatCategory)))
        {
            temp = temp.Replace("{" + item + "}", skillSet.entity.clone.GetStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
            temp = temp.Replace("{Max" + item + "}", skillSet.entity.clone.GetMaxStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
        }

        Expression ex = new Expression(temp);
        object obj = ex.Evaluate();

        if (obj.GetType() == typeof(double))
        {
            return (int)((double)ex.Evaluate());
        }
        else
        {
            return (int)ex.Evaluate();
        }
    }


    public int CalculateSkillDamageByIndex(int index)
    {
        string temp = skillDamageForms[index];
        foreach (var item in System.Enum.GetNames(typeof(StatCategory)))
        {
            temp = temp.Replace("{" + item + "}", skillSet.entity.clone.GetStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
            temp = temp.Replace("{Max" + item + "}", skillSet.entity.clone.GetMaxStat((StatCategory)System.Enum.Parse(typeof(StatCategory), item)).ToString());
        }

        Expression ex = new Expression(temp);
        object obj = ex.Evaluate();

        if (obj.GetType() == typeof(double))
        {
            return (int)((double)ex.Evaluate());
        }
        else
        {
            return (int)ex.Evaluate();
        }
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
        object obj = ex.Evaluate();

        if (obj.GetType() == typeof(double))
        {
            return (int)((double)ex.Evaluate());
        }
        else
        {
            return (int)ex.Evaluate();
        }
    }

    void Awake()
    {
        for (int i = 0; i < hitBoxDatas.Length; i++)
        {
            hitBoxDatas[i].LeftHitBox.skillData = this;
        }
        for (int i = 0; i < hitBoxDatas.Length; i++)
        {
            hitBoxDatas[i].RightHitBox.skillData = this;
        }

        skillSet = transform.parent.GetComponent<SkillSet>();
        foreach (var item in hitBoxDatas)
        {
            item.LeftHitBox.transform.parent.gameObject.SetActive(false);
        }

    }

    public void ActiveHitBox(bool isRight)
    {
        if (isRight)
        {
            hitBoxDatas[targetIndex].LeftHitBox.gameObject.SetActive(false);
            hitBoxDatas[targetIndex].RightHitBox.gameObject.SetActive(true);
        }
        else
        {
            hitBoxDatas[targetIndex].LeftHitBox.gameObject.SetActive(true);
            hitBoxDatas[targetIndex].RightHitBox.gameObject.SetActive(false);
        }
    }

}

[System.Serializable]
public class HitBoxData
{
    public HitBox LeftHitBox;
    public HitBox RightHitBox;
}