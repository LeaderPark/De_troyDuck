using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool direction; // false left, true right

    private EntityEvent entityEvent;

    private Entity entity;

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
			RaycastHit hit;
			if (Physics.SphereCast(transform.position + Vector3.up * 10, 2, Vector3.down, out hit, 20, LayerMask.GetMask(new string[] { "Enemy" })))
			{
                GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
                Entity target = hit.transform.GetComponent<Entity>();
                if (target.isDead)
                {
                    entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocityNoLock", new object[] { Vector3.zero, 0 });
                    entity.clone.Die();
                    entity.gameObject.layer = 7;
                    entity.gameObject.tag = "Enemy";
                    gameManager.Player = target.GetComponent<Player>();
                    gameManager.Player.enabled = true;
                    gameManager.Player.gameObject.layer = 6;
                    gameManager.Player.gameObject.tag = "Player";
                    
                    return;
                }
			}
        }
        if (inputManager.CheckMouseState(MouseButton.Right, ButtonState.Down) && !dashCool)
        {
            entity.GetProcessor(typeof(Processor.Move))?.AddCommand("SetVelocity", new object[]{new Vector3(inputX, 0, inputY).normalized, entity.clone.GetStat(StatCategory.Speed) * 4});
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
        yield return new WaitForSeconds(1f);
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