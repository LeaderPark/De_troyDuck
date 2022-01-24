using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool direction {get; set;} // false left, true right
    public Vector2 spawnPoint {get; set;}

    public EntityEvent entityEvent {get; private set;}

    public Entity entity {get; private set;}

    void Awake()
    {
        entity = GetComponent<Entity>();
        entityEvent = GetComponent<EntityEvent>();
        direction = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint, GetComponent<Entity>().entityData.activityRadius);
        Gizmos.color = Color.blue;
        Vector3 temp = transform.position;
        temp.y = 0;
        Gizmos.DrawWireSphere(temp, GetComponent<Entity>().entityData.searchRadius);
    }

}
