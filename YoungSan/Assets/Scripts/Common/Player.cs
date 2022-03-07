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

    void Update()
    {
        Process();
    }

    private bool dash;
    private bool dashCool;

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

        if (inputX > 0)
        {
            direction = true;
        }
        else if (inputX < 0)
        {
            direction = false;
        }


        //if (Input.GetMouseButtonDown(0))
        if(Input.GetKeyDown(KeyCode.Z))
        {
            entityEvent.CallEvent(EventCategory.DefaultAttack, new object[] { inputX, inputY, direction });
        }
        if (inputManager.CheckMouseState(MouseButton.Left, ButtonState.Down))
        {
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Ground" })))
			{
				Vector3 mousePos = hit.point - transform.position;
				bool attackDirection = (mousePos.x > 0f);
				direction = attackDirection;
				entityEvent.CallEvent(EventCategory.DefaultAttack, new object[] { mousePos.x, mousePos.z, attackDirection });
			}
		}
        if(inputManager.CheckKeyState(KeyCode.Q, ButtonState.Down))
        {
			RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 10, 2, Vector3.down, 20, LayerMask.GetMask(new string[] { "Enemy" }));
			float distance = 0;
            Entity target = null;
            foreach (RaycastHit hit in hits)
            {
                Entity hitEntity = hit.transform.GetComponent<Entity>();

                if (hitEntity.isDead)
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
                
                float hpRatio = uiManager.BackUpHpStat();
                float staminaRatio = uiManager.BackUpStaminaStat();
                entity.clone.Die();
                entity.gameObject.GetComponent<AudioListener>().enabled = false;
                entity.gameObject.layer = 7;
                entity.gameObject.tag = "Enemy";
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
                gameManager.Player.entity.clone.SetStat(StatCategory.Health, Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Health)) * hpRatio));
                //gameManager.Player.entity.clone.SetStat(StatCategory.Stamina,  Mathf.RoundToInt((float)(gameManager.Player.entity.clone.GetMaxStat(StatCategory.Stamina)) * staminaRatio));

                return;
            }
        }
        if (inputManager.CheckMouseState(MouseButton.Right, ButtonState.Down) && !dashCool && entity.clone.GetStat(StatCategory.Stamina) >= 50 && new Vector3(inputX, 0, inputY).normalized != Vector3.zero)
        {
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) * 4});
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("Lock", new object[]{0.35f});
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Dash"});
            entity.clone.SubStat(StatCategory.Stamina, 50);
            StartCoroutine(AttackVelocityTime(0.08f));
            dash = true;
            dashCool = true;
        }
        if (dash) return;

        entityEvent.CallEvent(EventCategory.Move, new object[]{inputX, inputY, direction});
    }
    
    private IEnumerator AttackVelocityTime(float time)
    {
        yield return new WaitForSeconds(time);
        dash = false;
        entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{Vector3.zero, 0});

        UIManager uIManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
        uIManager.skillinterface.time_coolTime = dashCoolTime;
        uIManager.skillinterface.Trigger_Skill(); //새롭게 추가 추후 수정

        yield return new WaitForSeconds(dashCoolTime);

        dashCool = false;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[]{"Ground"})))
        {
            Gizmos.DrawLine(transform.position, hit.point);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2);
    }

}