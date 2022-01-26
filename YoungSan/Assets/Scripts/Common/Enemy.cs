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
        entity.clone.SetStat(StatCategory.Health, (int)(entity.clone.GetMaxStat(StatCategory.Health) * 0.5f));
        entity.clone.SetStat(StatCategory.Attack, (int)(entity.clone.GetMaxStat(StatCategory.Attack) * 0.5f));
        entity.clone.SetStat(StatCategory.Speed, (int)(entity.clone.GetMaxStat(StatCategory.Speed) * 0.5f));
        
    }

}
