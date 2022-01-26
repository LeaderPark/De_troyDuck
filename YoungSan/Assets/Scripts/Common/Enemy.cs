using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool direction {get; set;} // false left, true right
    public Vector2 spawnPoint {get; set;}

    public EntityEvent entityEvent {get; private set;}

    public Entity entity {get; private set;}

    public SkillArea skillArea {get; private set;}

    void Awake()
    {
        entity = GetComponent<Entity>();
        entityEvent = GetComponent<EntityEvent>();
        skillArea = GetComponentInChildren<SkillArea>();
        direction = false;
    }

    void OnDrawGizmosSelected()
    {
        StateMachine.StateMachine stateMachine = GetComponent<StateMachine.StateMachine>();
        if (stateMachine != null && stateMachine.stateMachineData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint, stateMachine.stateMachineData.activityRadius);
            Gizmos.color = Color.blue;
            Vector3 temp = transform.position;
            temp.y = 0;
            Gizmos.DrawWireSphere(temp, stateMachine.stateMachineData.searchRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(temp, stateMachine.stateMachineData.distanceRadius);
        }
    }

}
