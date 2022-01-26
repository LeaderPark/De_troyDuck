using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    private Hashtable Processors {get; set;}

    public EntityData entityData;
    public Clone clone;

    public bool isDead;
    
    public Processor.Processor GetProcessor(Type processor)
    {
        if (Processors.ContainsKey(processor))
        {
            return Processors[processor] as Processor.Processor;
        }
        return null;
    }

    private void Process()
    {
        if (isDead) GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (isDead) return;
        foreach (Processor.Processor processor in Processors.Values)
        {
            processor.Process();
        }
    }
    
    void Awake()
    {
        Processors = new Hashtable();
        clone = new Clone(this, entityData);
        SettingProcessor();
    }

    public void Dead()
    {
        isDead = true;
        Player p = GetComponent<Player>();
        if (p != null) Destroy(p);
        Enemy e = GetComponent<Enemy>();
        if (e != null) Destroy(e);
        StateMachine.StateMachine s = GetComponent<StateMachine.StateMachine>();
        if (s != null) Destroy(s);
        GetComponent<Animator>().Play("Idle");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void Rebirth()
    {
        isDead = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void SettingProcessor()
    {
        if (GetComponent<Animator>() != null)
        {
            new Processor.Animate(Processors, GetComponent<Animator>());
        }
        if (GetComponent<Rigidbody>() != null)
        {
            new Processor.Move(Processors, GetComponent<Rigidbody>());
        }
        if (GetComponent<BoxCollider>() != null)
        {
            new Processor.Collision(Processors, GetComponent<BoxCollider>());
            new Processor.HitBody(Processors, clone);
        }
        if (GetComponentInChildren<SkillSet>() != null)
        {
            new Processor.Skill(Processors, GetComponentInChildren<SkillSet>());
        }
        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            new Processor.Sprite(Processors, GetComponent<SpriteRenderer>());
        }
    }

    float timeStack;

    void Update()
    {
        if (isDead)
        {
            timeStack += Time.deltaTime;
            if (timeStack >= 3f)
            {
                Destroy(gameObject);
            }
        }
        else timeStack = 0;
    }

    void LateUpdate()
    {
        Process();
    }
}
