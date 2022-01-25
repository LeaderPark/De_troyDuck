using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharEvent : EntityEvent
{
    private bool dontmove;
    private int attackStack;

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
        else
        {
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) / 2});
        }
        

        System.Action<float> end = (System.Action<float>)((float time) => 
        {
            if (time >= 0.9f)
            {
                dontmove = false;
                attackStack = 0;
            }
        });

        entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack1", end});
        entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack2", end});
        entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack3", end});
    }
    private void CallDefaultAttack(float inputX, float inputY, bool direction)
    {
        System.Action<float> attack2 = (System.Action<float>)((float time) => 
        {
            if (time >= 0.5f)
            {
                entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{1, direction, (System.Action)(() =>
                {
                    entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                    entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack2"});
                    dontmove = true;
                })});
                attackStack = 3;
            }
            else
            {
                attackStack = 1;
            }
        });
        System.Action<float> attack3 = (System.Action<float>)((float time) => 
        {
            if (time >= 0.2f)
            {
                entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{2, direction, (System.Action)(() =>
                {
                    entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                    entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack3"});
                    dontmove = true;
                })});
                attackStack = 5;
            }
            else
            {
                attackStack = 3;
            }
        });
        if (attackStack == 0)
        {
            entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{0, direction, (System.Action)(() =>
            {
                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack1"});
                entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});
                attackStack = 1;
                dontmove = true;
            })});
        }
        else if (attackStack == 1)
        {
            attackStack = 2;
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack1", attack2});
        }
        else if (attackStack == 3)
        {
            attackStack = 4;
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack2", attack3});
        }
    }
    private void CallSkill1()
    {
    }
    private void CallSkill2()
    {

    }
}
