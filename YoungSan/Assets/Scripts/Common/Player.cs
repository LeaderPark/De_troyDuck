using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool direction; // false left, true right
    private bool dontmove;

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
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Move_Right"});
                }
                else
                {
                    entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Move_Left"});
                }
            }
        
            entity.GetProcessor(typeof(Move))?.AddCommand("MoveToWard", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) * Time.deltaTime});
            entity.GetProcessor(typeof(Collision))?.AddCommand("SetCollider", new object[]{GetComponent<SpriteRenderer>().sprite});
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (direction)
            {
                entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack1_Right"});
            }
            else
            {
                entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack1_Left"});
            }
            entity.GetProcessor(typeof(Move))?.AddCommand("Stop", new object[]{});
            dontmove = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (direction)
            {
                entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack2_Right"});
            }
            else
            {
                entity.GetProcessor(typeof(Animate))?.AddCommand("Play", new object[]{"Attack2_Left"});
            }
            entity.GetProcessor(typeof(Move))?.AddCommand("Stop", new object[]{});
            dontmove = true;
        }

        if (direction)
        {
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClipEnd", new object[]{"Attack1_Right", (System.Action)(() => {dontmove = false;})});
        }
        else
        {
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClipEnd", new object[]{"Attack1_Left", (System.Action)(() => {dontmove = false;})});
        }

        if (direction)
        {
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClipEnd", new object[]{"Attack2_Right", (System.Action)(() => {dontmove = false;})});
        }
        else
        {
            entity.GetProcessor(typeof(Animate))?.AddCommand("CheckClipEnd", new object[]{"Attack2_Left", (System.Action)(() => {dontmove = false;})});
        }
    }
}