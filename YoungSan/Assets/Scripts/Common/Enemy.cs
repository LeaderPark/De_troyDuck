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

    void Start()
    {
        entity.clone.SubStat(StatCategory.Health, entity.clone.GetMaxStat(StatCategory.Health) / 2);
        entity.clone.SubStat(StatCategory.Attack, entity.clone.GetMaxStat(StatCategory.Attack) / 2);
        entity.clone.SubStat(StatCategory.Speed, entity.clone.GetMaxStat(StatCategory.Speed) / 2);
        
    }

}
