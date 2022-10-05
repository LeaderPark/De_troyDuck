using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool direction; // false left, true right

    private EntityEvent entityEvent;

    private Entity entity;

    public float dashCoolTime = 1f;

    public bool targeting;
    public EventCategory targetSkillCategory;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entityEvent = GetComponent<EntityEvent>();
        direction = false;
    }

    void OnDisable()
    {
        dash = false;
        dashCool = false;
    }

    private void Start()
    {
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
    }

    void Update()
    {
        Process();
    }

    private bool dash;
    private Coroutine dashCo;
    private bool dashCool;


    private bool skipMove;
    private void Process()
    {

        GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
        InputManager inputManager = ManagerObject.Instance.GetManager(ManagerType.InputManager) as InputManager;

        float inputX = 0;
        float inputY = 0;

        if (inputManager.CheckKeyState(KeyCode.D, ButtonState.Stay))
        {
            inputX = 1f;
        }
        else if (inputManager.CheckKeyState(KeyCode.A, ButtonState.Stay))
        {
            inputX = -1f;
        }
        if (inputManager.CheckKeyState(KeyCode.W, ButtonState.Stay))
        {
            inputY = 1f;
        }
        else if (inputManager.CheckKeyState(KeyCode.S, ButtonState.Stay))
        {
            inputY = -1f;
        }

        Entity prevTarget = null;
        if (targeting)
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1, Camera.main.transform.forward, 2000, LayerMask.GetMask(new string[] { "Enemy" }));
            if (hits.Length > 0)
            {
                float min = Vector2.Distance(Camera.main.WorldToScreenPoint(hits[0].transform.position), Input.mousePosition);
                prevTarget = hits[0].transform.GetComponent<Entity>();
                for (int i = 1; i < hits.Length; i++)
                {
                    float temp = Vector2.Distance(Camera.main.WorldToScreenPoint(hits[i].transform.position), Input.mousePosition);
                    if (min > temp)
                    {
                        min = temp;
                        prevTarget = hits[i].transform.GetComponent<Entity>();
                    }
                }
            }

            if (prevTarget)
            {
                SpriteRenderer sr = prevTarget.GetComponent<SpriteRenderer>();
                gameManager.targetSelect.sprite = sr.sprite;
                gameManager.targetSelect.flipX = sr.flipX;
                gameManager.targetSelect.transform.position = prevTarget.transform.position;
                gameManager.targetSelect.transform.localScale = prevTarget.transform.localScale;
            }
            else
            {
                gameManager.targetSelect.sprite = null;
            }
        }
        else
        {
            gameManager.targetSelect.sprite = null;
        }

        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Z))
        {
            entityEvent.CallEvent(EventCategory.DefaultAttack, inputX, inputY, direction, transform.position);
        }
        if (inputManager.CheckMouseState(MouseButton.Left, ButtonState.Down))
        {
            if (targeting)
            {
                if (prevTarget != null)
                {
                    SkillSet skillSet = GetComponentInChildren<SkillSet>();
                    skillSet.skillDatas[targetSkillCategory][skillSet.skillStackAmount[targetSkillCategory]].target = prevTarget;

                    Vector3 mousePos = prevTarget.transform.position - transform.position;
                    bool attackDirection = (mousePos.x > 0f);
                    direction = attackDirection;
                    entityEvent.CallEvent(targetSkillCategory, mousePos.x, mousePos.z, attackDirection, prevTarget.transform.position);
                    skipMove = true;

                    targeting = false;
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
                {
                    Vector3 mousePos = hit.point - transform.position;
                    bool attackDirection = (mousePos.x > 0f);
                    direction = attackDirection;
                    entityEvent.CallEvent(EventCategory.DefaultAttack, mousePos.x, mousePos.z, attackDirection, hit.point);
                    skipMove = true;
                }
            }
        }
        if (inputManager.CheckKeyState(KeyCode.E, ButtonState.Down))
        {
            SkillSet skillSet = GetComponentInChildren<SkillSet>();
            if (skillSet.skillDatas.ContainsKey(EventCategory.Skill1) && skillSet.skillDatas[EventCategory.Skill1][skillSet.skillStackAmount[EventCategory.Skill1]].targeting)
            {
                targeting = true;
                targetSkillCategory = EventCategory.Skill1;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
                {
                    Vector3 mousePos = hit.point - transform.position;
                    bool attackDirection = (mousePos.x > 0f);
                    direction = attackDirection;
                    entityEvent.CallEvent(EventCategory.Skill1, mousePos.x, mousePos.z, attackDirection, hit.point);
                    skipMove = true;
                }
            }
        }
        else if (inputManager.CheckKeyState(KeyCode.R, ButtonState.Down))
        {
            SkillSet skillSet = GetComponentInChildren<SkillSet>();
            if (skillSet.skillDatas.ContainsKey(EventCategory.Skill2) && skillSet.skillDatas[EventCategory.Skill2][skillSet.skillStackAmount[EventCategory.Skill2]].targeting)
            {
                targeting = true;
                targetSkillCategory = EventCategory.Skill2;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
                {
                    Vector3 mousePos = hit.point - transform.position;
                    bool attackDirection = (mousePos.x > 0f);
                    direction = attackDirection;
                    entityEvent.CallEvent(EventCategory.Skill2, mousePos.x, mousePos.z, attackDirection, hit.point);
                    skipMove = true;
                }
            }
        }
        else if (inputManager.CheckKeyState(KeyCode.F, ButtonState.Down))
        {
            SkillSet skillSet = GetComponentInChildren<SkillSet>();
            if (skillSet.skillDatas.ContainsKey(EventCategory.Skill3) && skillSet.skillDatas[EventCategory.Skill3][skillSet.skillStackAmount[EventCategory.Skill3]].targeting)
            {
                targeting = true;
                targetSkillCategory = EventCategory.Skill3;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
                {
                    Vector3 mousePos = hit.point - transform.position;
                    bool attackDirection = (mousePos.x > 0f);
                    direction = attackDirection;
                    entityEvent.CallEvent(EventCategory.Skill3, mousePos.x, mousePos.z, attackDirection, hit.point);
                    skipMove = true;
                }
            }
        }

        if (inputManager.CheckKeyState(KeyCode.E, ButtonState.Up))
        {
            if (targetSkillCategory == EventCategory.Skill1)
            {
                targeting = false;
            }
        }
        if (inputManager.CheckKeyState(KeyCode.R, ButtonState.Up))
        {
            if (targetSkillCategory == EventCategory.Skill2)
            {
                targeting = false;
            }
        }
        if (inputManager.CheckKeyState(KeyCode.F, ButtonState.Up))
        {
            if (targetSkillCategory == EventCategory.Skill3)
            {
                targeting = false;
            }
        }

        if (inputManager.CheckKeyState(KeyCode.Space, ButtonState.Down))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 10, 2, Vector3.down, 20, LayerMask.GetMask(new string[] { "Npc" }));
            float distance = 0;
            NPCTalk target = null;

            foreach (RaycastHit hit in hits)
            {
                NPCTalk hitNpc = hit.transform.GetComponent<NPCTalk>();
                if (hitNpc != null)
                {
                    if (target == null)
                    {
                        target = hitNpc;
                        distance = Vector2.Distance(new Vector2(hitNpc.transform.position.x, hitNpc.transform.position.z), new Vector2(transform.position.x, transform.position.z));
                    }
                    else
                    {
                        float temp = Vector2.Distance(new Vector2(hitNpc.transform.position.x, hitNpc.transform.position.z), new Vector2(transform.position.x, transform.position.z));
                        if (distance > temp)
                        {
                            target = hitNpc;
                            distance = temp;
                        }
                    }

                }
            }

            if (target != null)
            {
                Debug.Log(target);
                target.Talk();
            }
        }

        if (inputManager.CheckKeyState(KeyCode.Q, ButtonState.Down))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 10, 2, Vector3.down, 20, LayerMask.GetMask(new string[] { "Enemy" }));
            float distance = 0;
            Entity target = null;
            foreach (RaycastHit hit in hits)
            {
                Entity hitEntity = hit.transform.GetComponent<Entity>();

                if (hitEntity != null && hitEntity.isDead && !hitEntity.cantChange)
                {
                    if (target == null)
                    {
                        target = hitEntity;
                        distance = Vector2.Distance(new Vector2(hitEntity.transform.position.x, hitEntity.transform.position.z), new Vector2(transform.position.x, transform.position.z));
                    }
                    else
                    {
                        float temp = Vector2.Distance(new Vector2(hitEntity.transform.position.x, hitEntity.transform.position.z), new Vector2(transform.position.x, transform.position.z));
                        if (distance > temp)
                        {
                            target = hitEntity;
                            distance = temp;
                        }
                    }

                }
            }

            if (target != null)
            {
                UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

                float hpRatio = uiManager.statbar.BackUpHpStat();
                entity.Die(false);
                StopAllCoroutines();
                entity.gameObject.GetComponent<AudioListener>().enabled = false;
                entity.gameObject.layer = 7;
                entity.gameObject.tag = "Enemy";
                if (Mathf.Max(1, Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Health)) * gameManager.healthRate)) != gameManager.Player.entity.clone.GetStat(StatCategory.Health))
                {
                    gameManager.healthRate = hpRatio;
                }
                gameManager.Player = target.GetComponent<Player>();
                foreach (StatCategory stat in System.Enum.GetValues(typeof(StatCategory)))
                {
                    gameManager.Player.entity.clone.SetMaxStat(stat, gameManager.Player.entity.clone.GetPlayerMaxStat(stat));
                    gameManager.Player.entity.clone.SetStat(stat, gameManager.Player.entity.clone.GetPlayerMaxStat(stat));
                }
                gameManager.Player.GetComponent<AudioListener>().enabled = true;
                gameManager.Player.enabled = true;
                gameManager.Player.entity.isDead = false;
                gameManager.Player.entity.hitable = true;
                gameManager.Player.gameObject.layer = 6;
                gameManager.Player.gameObject.tag = "Player";
                gameManager.Player.entity.clone.SetStat(StatCategory.Health, Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetPlayerMaxStat(StatCategory.Health)) * gameManager.healthRate));
                gameManager.bell.Ring();
                uiManager.statbar.SetStatBar();
                //gameManager.Player.entity.clone.SetStat(StatCategory.Stamina,  Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Stamina)) * staminaRatio));

                uiManager.skillinterface.SetSkillDatas();
                return;
            }
        }
        if (inputManager.CheckMouseState(MouseButton.Right, ButtonState.Down) && !dashCool && entity.clone.GetStat(StatCategory.Stamina) >= 50 && new Vector3(inputX, 0, inputY).normalized != Vector3.zero)
        {
            entity.hitable = false;
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { new Vector3(inputX, 0, inputY).normalized, 24 });

            if (!(entity.GetProcessor(typeof(Processor.Animate)) as Processor.Animate).locking)
            {
                if (inputX > 0)
                {
                    direction = true;
                }
                else if (inputX < 0)
                {
                    direction = false;
                }
                entity.GetProcessor(typeof(Processor.Sprite))?.AddCommand("SetDirection", new object[] { direction });
            }
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("LockTime", new object[] { 0.0f });
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Move" });
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Dash" });
            entity.clone.SubStat(StatCategory.Stamina, 50);

            SkillSet skillSet = entity.GetComponentInChildren<SkillSet>();
            skillSet.StopSkill();

            if (dashCo != null) StopCoroutine(dashCo);
            dashCo = StartCoroutine(AttackVelocityTime(skillSet, 0.2f, new object[] { new Vector3(inputX, 0, inputY).normalized, 24 }));
            gameManager.AfterImage(entity, 0.2f);
            dash = true;
            dashCool = true;
            UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

        }

        // if (GetComponent<Rigidbody>().velocity.sqrMagnitude != 0)
        // {
        //     gameManager.bell.Move(direction);
        // }
        // else
        // {
        //     gameManager.bell.Idle();
        // }

        if (dash) return;

        if (skipMove && entityEvent.dontmove)
        {
            skipMove = false;
            return;
        }

        if (inputX > 0)
        {
            direction = true;
        }
        else if (inputX < 0)
        {
            direction = false;
        }

        entityEvent.CallEvent(EventCategory.Move, inputX, inputY, direction, transform.position);
    }


    private IEnumerator AttackVelocityTime(SkillSet skillSet, float time, object[] o)
    {
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        // uIManager.skillinterface.time_coolTime = dashCoolTime + time + 0.27f;
        // uIManager.skillinterface.Trigger_Skill(); //새롭게 추가 추후 수정

        PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
        Effect dashEffect = poolManager.GetObject("DashEffect").GetComponent<Effect>();
        dashEffect.transform.position = entity.transform.position + Vector3.up * 0.5f;

        dashEffect.Play("DashEffect");

        float timeStack = 0;

        while (timeStack < time)
        {
            timeStack += Time.deltaTime;

            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", o);
            yield return null;
        }

        dash = false;
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });

        yield return new WaitForSeconds(0.3f);
        entity.hitable = true;

        yield return new WaitForSeconds(dashCoolTime - 0.3f);

        dashCool = false;
    }
    public void ActiveScript(bool active)
    {
        gameObject.GetComponent<Player>().enabled = active;
        gameObject.GetComponent<Entity>().enabled = active;
        gameObject.GetComponent<EntityEvent>().enabled = active;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Animator>().Play("Idle");
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
        {
            Gizmos.DrawLine(transform.position, hit.point);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2);
    }

}