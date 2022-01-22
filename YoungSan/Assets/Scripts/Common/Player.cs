using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool direction; // false left, true right

    private EntityEvent entityEvent;

    private Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entityEvent = GetComponent<EntityEvent>();
        direction = false;
    }

    void Update()
    {
        Process();
    }


    private void Process()
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


        if (Input.GetMouseButtonDown(0))
        {
            bool attackDirection = (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.5f);
            direction = attackDirection;
            entityEvent.CallEvent(EventCategory.DefaultAttack, new object[]{inputX, inputY, attackDirection});
        }

        entityEvent.CallEvent(EventCategory.Move, new object[]{inputX, inputY, direction});

    }
}