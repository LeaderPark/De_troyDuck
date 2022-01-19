using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private bool direction; // false left, true right

    private Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
        direction = false;
    }
    
    void Update()
    {

        float inputX = Random.Range(-1, 2);
        float inputY = Random.Range(-1, 2);
        
        if (inputX == 0 && inputY == 0)
        {
            if (direction)
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Idle_Right"});
            }
            else
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Idle_Left"});
            }
        }
        else
        {   
            if (inputX > 0)
            {
                direction = true;
            }
            else if (inputX < 0)
            {
                direction = false;
            }
            if (direction)
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Move_Right"});
            }
            else
            {
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Play", new object[]{"Move_Left"});
            }
        }
        
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
        entity.GetProcessor(typeof(Processor.Collision))?.AddCommand("SetCollider", new object[]{GetComponent<SpriteRenderer>().sprite});
    }

}
