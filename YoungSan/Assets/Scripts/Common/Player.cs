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
        if (looock) return;
        Process();
    }

    private bool looock;

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
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2000, LayerMask.GetMask(new string[] { "Enemy" })))
			{
                entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Idle", (System.Action<bool, float>)((bool transition, float time)=>
                {
                    if (!transition)
                    {
                        if (hit.collider.gameObject.GetComponent<Entity>().isDead)
                        {
                            Vector3 mousePos = hit.point - transform.position;
                            if (Vector2.Distance(new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.z), new Vector2(transform.position.x, transform.position.z)) < 6)
                            {
                                StartCoroutine(ChangeBody(hit));
                            }
                        }
                    }
                })});
			}
		}

        entityEvent.CallEvent(EventCategory.Move, new object[]{inputX, inputY, direction});
    }

    IEnumerator ChangeBody(RaycastHit hit)
    {
        looock = true;
        entity.gameObject.layer = 0;

        entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("PlayNoLock", new object[]{"Change"});

        Entity hitEntity = hit.collider.gameObject.GetComponent<Entity>();
        hitEntity.Rebirth();

        bool nyan = false;
        while (!nyan)
        {
            entity.GetProcessor(typeof(Processor.Animate))?.AddCommand("CheckClipNoLock", new object[]{"Change", (System.Action<bool, float>)((bool transition, float time) =>
            {
                if (!transition && time >= 0.9f)
                {
                    nyan = true;
                }
            })});
            yield return null;
        }

        hitEntity.gameObject.AddComponent<Player>();
        hitEntity.GetComponent<SpriteRenderer>().color = Color.white;

        hitEntity.clone.SetStat(StatCategory.Health, hitEntity.clone.GetMaxStat(StatCategory.Health));
        hitEntity.clone.SetStat(StatCategory.Attack, hitEntity.clone.GetMaxStat(StatCategory.Attack));
        hitEntity.clone.SetStat(StatCategory.Speed, hitEntity.clone.GetMaxStat(StatCategory.Speed));
        hitEntity.gameObject.layer = 6;
        FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = hitEntity.transform;

        entity.gameObject.layer = 7;
        entity.clone.SetStat(StatCategory.Health, 0);
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