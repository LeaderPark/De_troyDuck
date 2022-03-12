using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvent : MonoBehaviour
{
    public Entity entity;

    public void CallEvent(EventCategory eventCategory, object[] parameters)
    {                                                                                                                                                                   
        this.GetType().GetMethod(string.Concat("Call", eventCategory.ToString()), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(this, parameters);
    }                                                                                                                                                                   

    protected virtual void Awake()
    {
        dontmove = false;
        attackStack = new Dictionary<EventCategory, int>();
        maxAttackStack = new Dictionary<EventCategory, int>();
        attackAnimation = new Dictionary<EventCategory, string[]>();
        attackTransitionTime = new Dictionary<EventCategory, (float, float)[]>();
        attackIndex = new Dictionary<EventCategory, int[]>();
        attackProcess = new Dictionary<EventCategory, System.Action<float, float>[]>();
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        foreach (EventCategory category in maxAttackStack.Keys)
        {
            attackStack[category] = 0;
            StartCoroutine(AttackEndCheck(category));
        }
    }
    
    private bool dontmove;
    protected void CallMove(float inputX, float inputY, bool direction)
    {
        if (!dontmove)
        {
            entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
            if (inputX == 0 && inputY == 0)
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Idle"});
            }
            else
            {   
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Move"});
            }

            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
            entity.GetProcessor(typeof(Processor.Collision))?.AddCommand("SetCollider", new object[]{GetComponent<SpriteRenderer>().sprite});
        }
        
    }

    private Dictionary<EventCategory, int> attackStack;
    protected Dictionary<EventCategory, int> maxAttackStack;
    protected Dictionary<EventCategory, string[]> attackAnimation;
    protected Dictionary<EventCategory, (float, float)[]> attackTransitionTime;
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
        if (!attackStack.ContainsKey(category)) return;
        if (attackStack[category] == 0)
        {
            entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{category, attackIndex[category][0], new Vector2(inputX, inputY), direction, (System.Action)(() =>
            {
                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{attackAnimation[category][0]});
                entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
                attackProcess[category][0]?.Invoke(inputX, inputY);
                dontmove = true;
            })});
            attackStack[category] = 1;
        }
        else
        {
            for (int i = 0; i < maxAttackStack[category] - 1; i++)
            {
                if (attackStack[category] == i + 1)
                {
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{attackAnimation[category][i], (System.Action<bool, float>)((bool transition, float time) => 
                    {
                        if (!transition && time >= attackTransitionTime[category][i].Item1 && time <= attackTransitionTime[category][i].Item2 || transition)
                        {
                            entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{category, attackIndex[category][i + 1], new Vector2(inputX, inputY), direction, (System.Action)(() =>
                            {
                                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{attackAnimation[category][i + 1]});
                                entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
                                attackProcess[category][i + 1]?.Invoke(inputX, inputY);
                                dontmove = true;
                            })});
                            attackStack[category] = i + 2;
                        }
                        else
                        {
                            attackStack[category] = i + 1;
                        }
                    })});

                    break;
                }
            }
        }
    }

    protected void Dash(float inputX, float inputY, float speed, float time)
    {
        StartCoroutine(AttackVelocityTime(inputX, inputY, speed, time));
    }

    private IEnumerator AttackVelocityTime(float inputX, float inputY, float speed, float time)
    {
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
                if (!transition && time >= 1f || transition)
                {
                    attackStack[category] = 0;
                    dontmove = false;
                }
            });

            foreach (string animation in attackAnimation[category])
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{animation, end});
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
