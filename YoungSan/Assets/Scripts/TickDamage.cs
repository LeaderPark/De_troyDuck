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

    public string tickDamageForm;

    float timeStack;

    public void SetData(Entity sourceEntity, Entity targetEntity, float delay, float time)
    {
        this.sourceEntity = sourceEntity;
        this.targetEntity = targetEntity;
        this.delay = delay;
        this.count = (int)(time / delay);

        timeStack = 0;
    }

    void Update()
    {
        if (count == 0 || targetEntity.isDead) gameObject.SetActive(false);
        timeStack += Time.deltaTime;
        transform.position = targetEntity.transform.position;

        if (timeStack >= delay)
        {
            timeStack = 0;
            targetEntity?.GetProcessor(typeof(Processor.HitBody))?.AddCommand("DamageOnBody", new object[]{CalculateTickDamage(), sourceEntity});
            count--;
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
        return (int)ex.Evaluate();
    }
}
