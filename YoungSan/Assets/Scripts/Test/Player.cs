using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float moveSpeed;

    void Start()
    {
        new Move(Processors, GetComponent<Rigidbody>());
        new Animate(Processors, transform.GetComponent<Animator>());
        direction = false;
    }

    private bool direction; // false left, true right

    void Update()
    {
        MoveProcess();
    }


    private void MoveProcess()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX == 0 && inputY == 0)
        {
            if (direction)
            {
                GetProcessor("Animate").AddCommand("Play", new object[]{"Idle_Right"});
            }
            else
            {
                GetProcessor("Animate").AddCommand("Play", new object[]{"Idle_Left"});
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
                GetProcessor("Animate").AddCommand("Play", new object[]{"Move_Right"});
            }
            else
            {
                GetProcessor("Animate").AddCommand("Play", new object[]{"Move_Left"});
            }
        }
        
        GetProcessor("Move").AddCommand("MoveToWard", new object[]{new Vector3(inputX, 0, inputY).normalized, moveSpeed * Time.deltaTime});

    }
}