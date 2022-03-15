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
    public bool hitable;
        
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
        foreach (Processor.Processor processor in Processors.Values)
        {
            processor.Process();
        }
    }
    
    void Awake()
    {
        Processors = new Hashtable();
        clone = new Clone(this, entityData);
        isDead = false;
        hitable = true;
        SettingProcessor();
    }

    public void Die()
    {
        isDead = true;
        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = false;
        }
        if (GetComponent<Enemy>() != null)
        {
            GetComponent<Enemy>().enabled = false;
        }
        if (GetComponent<StateMachine.StateMachine>() != null)
        {
            GetComponent<StateMachine.StateMachine>().enabled = false;
        }
        GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[]{ Vector3.zero, 0 });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[]{});
        GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0f});
        GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Die"});
    }

	private void OnDrawGizmos()
	//private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position+new Vector3(0,entityData.uiPos,0), new Vector3(0.25f, 0.25f, 0.25f));
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
            new Processor.HitBody(Processors, this);
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

    void LateUpdate()
    {
        Process();
    }


    float staminaCount;
    void Update()
    {
        if (staminaCount <= 0f)
        {
            int temp = Mathf.RoundToInt((clone.GetMaxStat(StatCategory.Stamina) - clone.GetStat(StatCategory.Stamina)) * Time.deltaTime * (1f / 2f));
            clone.AddStat(StatCategory.Stamina, Mathf.Clamp(temp, 1, temp));
        }
        else
        {
            staminaCount -= Time.deltaTime;
        }
    }

    public void ResetStaminaCount()
    {
        staminaCount = 2f;
    }
}
