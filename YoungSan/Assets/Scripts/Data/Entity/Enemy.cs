using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public StateMachine stateMachine;

    public float moveSpeed;

    private bool direction; // false left, true right

    void Start()
    {
        new Move(Processors, GetComponent<Rigidbody>());
        new Animate(Processors, GetComponent<Animator>());
        direction = false;
    }

    void Update()
    {
        MoveProcess();
    }


    private void MoveProcess()
    {
    }
}
