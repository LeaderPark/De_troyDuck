using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool direction; // false left, true right

    private EntityEvent entityEvent;

    private Entity entity;

    public float dashCoolTime = 1f;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entityEvent = GetComponent<EntityEvent>();
        direction = false;
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


        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Z))
        {
            entityEvent.CallEvent(EventCategory.DefaultAttack, inputX, inputY, direction, transform.position);
        }
        if (inputManager.CheckMouseState(MouseButton.Left, ButtonState.Down))
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
        if (inputManager.CheckKeyState(KeyCode.E, ButtonState.Down))
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
        if (inputManager.CheckKeyState(KeyCode.R, ButtonState.Down))
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
        if (inputManager.CheckKeyState(KeyCode.F, ButtonState.Down))
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

                if (hitEntity != null && hitEntity.isDead)
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
                GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
                UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

                float hpRatio = uiManager.statbar.BackUpHpStat();
                entity.Die(false);
                StopAllCoroutines();
                entity.gameObject.GetComponent<AudioListener>().enabled = false;
                entity.gameObject.layer = 7;
                entity.gameObject.tag = "Enemy";
                if (Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Health)) * gameManager.healthRate) != gameManager.Player.entity.clone.GetStat(StatCategory.Health))
                {
                    gameManager.healthRate = hpRatio;
                }
                gameManager.Player = target.GetComponent<Player>();
                foreach (StatCategory stat in System.Enum.GetValues(typeof(StatCategory)))
                {
                    gameManager.Player.entity.clone.SetStat(stat, gameManager.Player.entity.clone.GetMaxStat(stat));
                }
                gameManager.Player.GetComponent<AudioListener>().enabled = true;
                gameManager.Player.enabled = true;
                gameManager.Player.entity.isDead = false;
                gameManager.Player.gameObject.layer = 6;
                gameManager.Player.gameObject.tag = "Player";
                gameManager.Player.entity.clone.SetStat(StatCategory.Health, Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Health)) * gameManager.healthRate));
                uiManager.statbar.SetStatBar();
                //gameManager.Player.entity.clone.SetStat(StatCategory.Stamina,  Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Stamina)) * staminaRatio));

                uiManager.skillinterface.SetSkillDatas();
                return;
            }
        }
        if (inputManager.CheckMouseState(MouseButton.Right, ButtonState.Down) && entity.clone.GetStat(StatCategory.Stamina) >= 50 && new Vector3(inputX, 0, inputY).normalized != Vector3.zero)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            entity.hitable = false;
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { new Vector3(inputX, 0, inputY).normalized, 24 });
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[] { 0.0f });
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[] { "Dash" });
            entity.clone.SubStat(StatCategory.Stamina, 50);

            SkillSet skillSet = entity.GetComponentInChildren<SkillSet>();
            skillSet.StopSkill();

            if (dashCo != null) StopCoroutine(dashCo);
            dashCo = StartCoroutine(AttackVelocityTime(0.2f));
            gameManager.AfterImage(entity, 0.47f);
            dash = true;
            dashCool = true;
            UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;

        }
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


    private IEnumerator AttackVelocityTime(float time)
    {
        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        // uIManager.skillinterface.time_coolTime = dashCoolTime + time + 0.27f;
        // uIManager.skillinterface.Trigger_Skill(); //새롭게 추가 추후 수정

        yield return new WaitForSeconds(time);
        dash = false;
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });

        yield return new WaitForSeconds(0.27f);
        entity.hitable = true;

        yield return new WaitForSeconds(dashCoolTime);

        dashCool = false;
    }
    public void ActiveScript(bool active)
    {
        gameObject.GetComponent<Player>().enabled = active;
        gameObject.GetComponent<Entity>().enabled = active;
        gameObject.GetComponent<EntityEvent>().enabled = active;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
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