using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    private Hashtable Processors { get; set; }

    public EntityStatusAilment entityStatusAilment;
    public EntityData entityData;
    public Clone clone;
    public Dictionary<StatCategory, int> extraStat;

    public bool isDead;
    public bool hitable;
    public bool cantChange;
    public Action dead = null;

    public bool isGround;

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
        extraStat = new Dictionary<StatCategory, int>();
        isDead = false;
        hitable = true;
        SettingProcessor();

        extraStat[StatCategory.Health] = 0;
        extraStat[StatCategory.Attack] = 0;
        extraStat[StatCategory.Stamina] = 0;
        extraStat[StatCategory.Speed] = 0;
    }

    public void SetHp(float hp)
    {
        clone.SetStat(StatCategory.Health, (int)(clone.GetMaxStat(StatCategory.Health) * hp));
        if (clone.GetStat(StatCategory.Health) > 0)
        {
            isDead = false;
        }
    }

    public void DieEvent(bool isDie = true)
    {
        if (gameObject.CompareTag("Player"))
        {
            return;
        }
        clone.SetStat(StatCategory.Health, 0);

        dead?.Invoke();
        hitable = false;

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = false;
        }
        if (GetComponent<Enemy>() != null)
        {
            GetComponent<Enemy>().enabled = false;
            StateMachine.StateMachine.fight.Remove(this);
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

            questManager.ResetQuests();
            uimanager.bossName.gameObject.SetActive(false);
            uimanager.bossStatbar.gameObject.SetActive(false);
            uimanager.UISetActiveFalse();

            gameManager.deathWindow.TurnOnWindow(
                () =>
                {
                    uimanager.important = false;
                    dataManager.Load();
                }
            );
        }
        entityStatusAilment.DeActiveAll();
        GetProcessor(typeof(Processor.Move))?.AddCommand("LockTime", new object[] { 1f });
        GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("Reset", new object[] { });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
        GetProcessor(typeof(Processor.Animate))?.AddCommand("LockTime", new object[] { 1f });
        GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Die" });
        StartCoroutine(DieAnimationComplete());
    }

    public void Die(bool isDie = true)
    {
        dead?.Invoke();
        hitable = false;
        isDead = true;

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = false;
        }
        if (GetComponent<Enemy>() != null)
        {
            GetComponent<Enemy>().enabled = false;
            StateMachine.StateMachine.fight.Remove(this);
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

            questManager.ResetQuests();
            uimanager.bossName.gameObject.SetActive(false);
            uimanager.bossStatbar.gameObject.SetActive(false);
            uimanager.UISetActiveFalse();

            gameManager.deathWindow.TurnOnWindow(
                () =>
                {
                    uimanager.important = false;
                    dataManager.Load();
                }
            );
        }
        entityStatusAilment?.DeActiveAll();
        GetProcessor(typeof(Processor.Move))?.AddCommand("LockTime", new object[] { 1f });
        GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("Reset", new object[] { });
        GetProcessor(typeof(Processor.Skill))?.AddCommand("StopSkill", new object[] { });
        GetProcessor(typeof(Processor.Animate))?.AddCommand("LockTime", new object[] { 0f });
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

        RaycastHit hit;

        if (entityStatusAilment != null)
        {
            if (entityStatusAilment.GetEntityStatus(typeof(Airbone)).Activated()) return;
        }

        Vector3 pos = transform.position;
        if (Physics.Raycast(new Ray(new Vector3(pos.x, 1000, pos.z), Vector3.down), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
        {
            pos.y = hit.point.y;
            transform.position = pos;
        }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            isGround = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            isGround = false;
        }
    }

    public void ResetStaminaCount()
    {
        staminaCount = 1f;
    }
}

