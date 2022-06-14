using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    private Hashtable Processors { get; set; }

    public EntityData entityData;
    public Clone clone;

    public bool isDead;
    public bool hitable;
    public Action dead = null;

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
    public void SetHp(float hp)
    {
        clone.SetStat(StatCategory.Health, (int)(clone.GetMaxStat(StatCategory.Health) * hp));
        if (clone.GetStat(StatCategory.Health) > 0)
        {
            isDead = false;
        }
    }
    public void Die(bool isDie = true)
    {
        dead?.Invoke();
        hitable = false;

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
        if (gameObject.CompareTag("Player") && isDie)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            DataManager dataManager = ManagerObject.Instance.GetManager(ManagerType.DataManager) as DataManager;
            UIManager uimanager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            QuestManager questManager = ManagerObject.Instance.GetManager(ManagerType.QuestManager) as QuestManager;

            questManager.AllResetQuests();
            uimanager.bossStatbar.gameObject.SetActive(false);
            uimanager.UISetActiveFalse();

            gameManager.deathWindow.TurnOnWindow(
                () =>
                {
                    dataManager.Load();
                }
            );
        }
        GetProcessor(typeof(Processor.Move))?.AddCommand("Lock", new object[] { 1f });
        GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("Reset", new object[] { });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
        GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[] { 0f });
        GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Die" });
        StartCoroutine(DieAnimationComplete());
    }

    IEnumerator DieAnimationComplete()
    {
        bool b = false;
        while (!b)
        {
            GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Die", (System.Action<bool, float>)((bool transition, float time) =>
            {
                if (!transition && time >= 1f)
                {
                    b = true;
                }
            })});
            yield return null;
        }
        hitable = true;
        isDead = true;
    }

    //private void OnDrawGizmos()
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position + new Vector3(0, entityData.uiPos, 0), new Vector3(0.25f, 0.25f, 0.25f));
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
            int temp = Mathf.RoundToInt(clone.GetMaxStat(StatCategory.Stamina) * Time.deltaTime * 0.5f);
            clone.AddStat(StatCategory.Stamina, Mathf.Clamp(temp, 1, temp));
        }
        else
        {
            staminaCount -= Time.deltaTime;
        }
    }

    public void ResetStaminaCount()
    {
        staminaCount = 1f;
    }
}

