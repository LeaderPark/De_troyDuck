using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMountainBanditEvent : EntityEvent
{
    private bool dontmove;
    private int attackStack;

    IEnumerator endCheck;
    

    protected override void Awake()
    {
        dontmove = false;
        attackStack = 0;
    }

    private void CallMove(float inputX, float inputY, bool direction)
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
    private void CallDefaultAttack(float inputX, float inputY, bool direction)
    {
        System.Action<bool, float> attack2 = (System.Action<bool, float>)((bool transition, float time) => 
        {
            if (!transition && time >= 0.6f && time <= 0.9f || transition)
            {
                entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{1, new Vector2(inputX, inputY), direction, (System.Action)(() =>
                {
                    entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack2"});
                    entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) * 2});
                    StartCoroutine(AttackVelocityTime(0.08f));
                    dontmove = true;
                })});
                attackStack = 2;
            }
            else
            {
                attackStack = 1;
            }
        });
        if (attackStack == 0)
        {
            entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{0, new Vector2(inputX, inputY), direction, (System.Action)(() =>
            {
                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack1"});
                entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
                dontmove = true;
            })});
            attackStack = 1;
        }
        else if (attackStack == 1)
        {
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack1", attack2});
        }
        if (endCheck == null)
        {
            endCheck = AttackEndCheck();
            StartCoroutine(endCheck);
        }
    }

    private IEnumerator AttackVelocityTime(float time)
    {
        yield return new WaitForSeconds(time);
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
    }

    private IEnumerator AttackEndCheck()
    {
        while (true)
        {
            System.Action<bool, float> end = (System.Action<bool, float>)((bool transition, float time) => 
            {
                if (!transition && time >= 1f || transition)
                {
                    attackStack = 0;
                    dontmove = false;
                }
            });

            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack1", end});
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack2", end});
            yield return null;
        }
    }

    private void CallSkill1()
    {
    }
    private void CallSkill2()
    {

    }
}