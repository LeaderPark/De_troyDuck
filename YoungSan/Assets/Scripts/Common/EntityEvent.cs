using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvent : MonoBehaviour
{
    [HideInInspector]
    public Entity entity;
    [HideInInspector]
    public SkillSet skillSet;

    public void CallEvent(EventCategory eventCategory, object[] parameters)
    {
        this.GetType().GetMethod(string.Concat("Call", eventCategory.ToString()), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(this, parameters);
    }                                                                                                                                                                   

    protected virtual void Awake()
    {
        dontmove = false;
        maxAttackStack = new Dictionary<EventCategory, int>();
        attackIndex = new Dictionary<EventCategory, int[]>();
        attackProcess = new Dictionary<EventCategory, System.Action<float, float>[]>();
        entity = GetComponent<Entity>();
        skillSet = entity.GetComponentInChildren<SkillSet>();
    }

    private void Start()
    {
        foreach (EventCategory category in maxAttackStack.Keys)
        {
            StartCoroutine(AttackEndCheck(category));
        }
    }
    
    public bool dontmove;
    public bool reservate;
    protected void CallMove(float inputX, float inputY, bool direction)
    {
        if (!dontmove)
        {
            entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
            if (inputX == 0 && inputY == 0)
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Idle", false});
            }
            else
            {   
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Move", false});
            }

            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
            entity.GetProcessor(typeof(Processor.Collision))?.AddCommand("SetCollider", new object[]{GetComponent<SpriteRenderer>().sprite});
        }
        
    }

    protected Dictionary<EventCategory, int> maxAttackStack;
    protected Dictionary<EventCategory, int[]> attackIndex;
    protected Dictionary<EventCategory, System.Action<float, float>[]> attackProcess;
    protected void CallDefaultAttack(float inputX, float inputY, bool direction)
    {
        AttackSkillEvent(inputX, inputY, direction, EventCategory.DefaultAttack);
    }
    protected void CallSkill1(float inputX, float inputY, bool direction)
    {
        AttackSkillEvent(inputX, inputY, direction, EventCategory.Skill1);
    }
    protected void CallSkill2(float inputX, float inputY, bool direction)
    {
        AttackSkillEvent(inputX, inputY, direction, EventCategory.Skill2);
    }
    protected void CallSkill3(float inputX, float inputY, bool direction)
    {
        AttackSkillEvent(inputX, inputY, direction, EventCategory.Skill3);
    }


    private void AttackSkillEvent(float inputX, float inputY, bool direction, EventCategory category)
    {
        if (!skillSet.skillStackAmount.ContainsKey(category)) return;

        entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{category, attackIndex[category][skillSet.skillStackAmount[category]], new Vector2(inputX, inputY), direction, (System.Action)(() =>
        {
            entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{skillSet.skillDatas[category][skillSet.skillStackAmount[category]].skill.name, true});
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
            attackProcess[category][skillSet.skillStackAmount[category]]?.Invoke(inputX, inputY);
            dontmove = true;
            reservate = true;
            skillSet.skillStackAmount[category] = (skillSet.skillStackAmount[category] + 1) % maxAttackStack[category];
        })});
    }

    protected void Dash(float inputX, float inputY, float speed, float startTime, float time)
    {
        StartCoroutine(AttackVelocityTime(inputX, inputY, speed, startTime, time));
    }

    private IEnumerator AttackVelocityTime(float inputX, float inputY, float speed, float startTime, float time)
    {
        yield return new WaitForSeconds(startTime);
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, speed});
        yield return new WaitForSeconds(time);
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
    }

    private IEnumerator AttackEndCheck(EventCategory category)
    {
        while (true)
        {
            System.Action<bool, float> end = (System.Action<bool, float>)((bool transition, float time) => 
            {
                if (!transition && time == 0f)
                {
                    reservate = false;
                }
                else if (!transition && time >= 1f)
                {
                    if (reservate) return;
                    skillSet.skillStackAmount[category] = 0;
                    dontmove = false;
                }
                else if (transition)
                {
                    reservate = false;
                    skillSet.skillStackAmount[category] = 0;
                    dontmove = false;
                }
            });

            foreach (var data in skillSet.skillDatas[category])
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{data.skill.name, end});
            }
            yield return null;
        }
    }
}

public enum EventCategory
{
    Move,
    DefaultAttack,
    Skill1,
    Skill2,
    Skill3
}
