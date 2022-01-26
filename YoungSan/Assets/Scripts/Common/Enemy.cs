using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool direction {get; set;} // false left, true right
    public Vector2 spawnPoint;

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

}
