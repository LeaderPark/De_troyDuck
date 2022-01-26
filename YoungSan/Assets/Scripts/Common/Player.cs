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
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

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
            //RaycastHit hit;
            //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[]{"Ground"})))
            //{
            //    Vector3 mousePos = hit.point - transform.position;
            //    bool attackDirection = (mousePos.x > 0f);
            //    direction = attackDirection;
            //    entityEvent.CallEvent(EventCategory.DefaultAttack, new object[]{mousePos.x, mousePos.z, attackDirection});
            //}
        }
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetMouseButtonDown(1))
        {
            if (entity.isDead) return;
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 2000, LayerMask.GetMask(new string[] { "Enemy" }));
			for (int i = 0; i < hits.Length; i++)
			{
                if (hits[i].collider.gameObject.GetComponent<Entity>().isDead)
                {
                    entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Idle", (System.Action<bool, float>)((bool transition, float time)=>
                    {
                        if (!transition)
                        {
                            if (hits[i].collider.gameObject.GetComponent<Entity>().isDead)
                            {
                                Vector3 mousePos = hits[i].point - transform.position;
                                if (Vector2.Distance(new Vector2(hits[i].collider.transform.position.x, hits[i].collider.transform.position.z), new Vector2(transform.position.x, transform.position.z)) < 6)
                                {
                                    Entity hitEntity = hits[i].collider.gameObject.GetComponent<Entity>();
                                    hitEntity.Rebirth();
                                    hitEntity.gameObject.AddComponent<Player>();

                                    hitEntity.clone.SetStat(StatCategory.Health, hitEntity.clone.GetMaxStat(StatCategory.Health));
                                    hitEntity.clone.SetStat(StatCategory.Attack, hitEntity.clone.GetMaxStat(StatCategory.Attack));
                                    hitEntity.clone.SetStat(StatCategory.Speed, hitEntity.clone.GetMaxStat(StatCategory.Speed));
                                    hitEntity.gameObject.layer = 6;
                                    FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = hitEntity.transform;

                                    entity.gameObject.layer = 7;
                                    entity.clone.SetStat(StatCategory.Health, 0);
                                }
                            }
                        }
                    })});
                    break;
                }
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