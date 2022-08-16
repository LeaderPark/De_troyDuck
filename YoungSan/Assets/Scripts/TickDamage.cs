using System.Collections;
using System.Collections.Generic;
using NCalc;
using UnityEngine;

public class TickDamage : MonoBehaviour
{
    Entity sourceEntity;
    Entity targetEntity;
    float delay;
    int count;

    string tickDamageForm;

    float timeStack;

    public void SetData(Entity sourceEntity, Entity targetEntity, float delay, float time, string tickDamageForm)
    {
        this.sourceEntity = sourceEntity;
        this.targetEntity = targetEntity;
        this.tickDamageForm = tickDamageForm;
        this.delay = delay;
        this.count = (int)(time / delay);

        timeStack = 0;

        if (count > 0)
        {
            GetComponent<UnityEngine.VFX.VisualEffect>().enabled = true;
        }
    }

    void Update()
    {
        if (targetEntity == null)
        {
            GetComponent<UnityEngine.VFX.VisualEffect>().enabled = false;
            gameObject.SetActive(false);
        }

        if (count > 0)
        {
            timeStack += Time.deltaTime;
            transform.position = targetEntity.transform.position;

            if (timeStack >= delay)
            {
                timeStack = 0;
                targetEntity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[] { CalculateTickDamage(), sourceEntity });
                count--;
            }
            if (count == 0 || targetEntity.isDead)
            {
                GetComponent<UnityEngine.VFX.VisualEffect>().enabled = false;
                enabled = false;
            }
        }
    }

    private int CalculateTickDamage()
    {
        string temp = tickDamageForm;
        SkillSet skillSet = sourceEntity.GetComponentInChildren<SkillSet>();

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
}
