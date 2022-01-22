using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyEvent : EntityEvent
{
    private void CallMove(float inputX, float inputY, bool direction)
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
