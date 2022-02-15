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


    private void Process()
    {
        InputManager inputManager = ManagerObject.Instance.GetManager(ManagerType.InputManager) as InputManager;

        float inputX = 0;
        float inputY = 0;

        if (inputManager.GetKeyState(KeyCode.D) == ButtonState.Stay)
        {
            inputX = 1f;
        }
        else if (inputManager.GetKeyState(KeyCode.A) == ButtonState.Stay)
        {
            inputX = -1f;
        }
        if (inputManager.GetKeyState(KeyCode.W) == ButtonState.Stay)
        {
            inputY = 1f;
        }
        else if (inputManager.GetKeyState(KeyCode.S) == ButtonState.Stay)
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
        if (inputManager.GetMouseState(MouseButton.Left) == ButtonState.Down)
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

        entityEvent.CallEvent(EventCategory.Move, new object[]{inputX, inputY, direction});
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[]{"Ground"})))
        {
            Gizmos.DrawLine(transform.position, hit.point);
        }
    }
}