using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
        float inputX = Random.Range(-1, 2);
        float inputY = Random.Range(-1, 2);

        if (inputX > 0)
        {
            direction = true;
        }
        else if (inputX < 0)
        {
            direction = false;
        }

        entityEvent.CallEvent(EventCategory.Move, new object[]{inputX, inputY, direction});

    }

}
