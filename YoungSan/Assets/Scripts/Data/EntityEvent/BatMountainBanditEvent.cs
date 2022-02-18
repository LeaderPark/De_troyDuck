using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMountainBanditEvent : EntityEvent
{
    private bool dontmove;

    IEnumerator endCheck;
    private bool dontAttack;

    protected override void Awake()
    {
        dontmove = false;
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
        if (!dontAttack)
        {
            entity.GetProcessor(typeof(Processor.Skill))?.AddCommand("UseSkill", new object[]{0, new Vector2(inputX, inputY), direction, (System.Action)(() =>
            {
                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[]{direction});
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Attack"});
                entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(0, 0, 0).normalized, 0});
                dontmove = true;
                dontAttack = true;
            })});
            if (endCheck == null)
            {
                endCheck = AttackEndCheck();
                StartCoroutine(endCheck);
            }
        }
    }

    private IEnumerator AttackEndCheck()
    {
        while (true)
        {
            System.Action<bool, float> end = (System.Action<bool, float>)((bool transition, float time) => 
            {
                if (!transition && time >= 1f || transition)
                {
                    dontmove = false;
                    dontAttack = false;
                }
            });

            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Attack", end});
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
