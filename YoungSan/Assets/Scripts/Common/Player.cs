using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool direction; // false left, true right
    private bool dontmove;
    private int attackStack;

    private Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
        direction = false;
        dontmove = false;
    }

    void Update()
    {
        MoveProcess();
    }


    private void MoveProcess()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX > 0)
        {
            direction = true;
        }
        else if (inputX < 0)
        {
            direction = false;
        }

        if (!dontmove)
        {
            if (inputX == 0 && inputY == 0)
            {
                if (direction)
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Idle_Right"});
                }
                else
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Idle_Left"});
                }
            }
            else
            {   
                if (direction)
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Move_Right"});
                }
                else
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Move_Left"});
                }
            }
        
            entity.GetProcessor(typeof(Move))?.AddCommand("MoveToWard", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
            entity.GetProcessor(typeof(Collision))?.AddCommand("SetCollider", new object[]{GetComponent<SpriteRenderer>().sprite});
        }

        if (Input.GetMouseButtonDown(0))
        {

            System.Action<float> attack2 = (System.Action<float>)((float time) => 
            {
                if (time >= 0.2f)
                {
                    attackStack = 3;
                    entity.GetProcessor(typeof(Move))?.AddCommand("MoveToWard", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed)});
                    if (direction)
                    {
                        entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack2_Right"});
                    }
                    else
                    {
                        entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack2_Left"});
                    }
                    dontmove = true;
                }
                else
                {
                    attackStack = 1;
                }
            });
            if (attackStack == 0)
            {
                if (direction)
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack1_Right"});
                }
                else
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack1_Left"});
                }
                attackStack = 1;
                entity.GetProcessor(typeof(Move))?.AddCommand("Stop", new object[]{});
                dontmove = true;
            }
            else
            {
                attackStack = 2;
                entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack1_Right", attack2});
                entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack1_Left", attack2});
            }
        }

        System.Action<float> end = (System.Action<float>)((float time) => 
        {
            if (time >= 0.8f)
            {
                dontmove = false;
                attackStack = 0;
            }
        });

        if (attackStack != 2)
        {
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack1_Right", end});
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack1_Left", end});
        }
        entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack2_Right", end});
        entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClip", new object[]{"Attack2_Left", end});
    }
}