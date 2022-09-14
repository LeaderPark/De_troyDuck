using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Pursue : State
    {
        BoxCollider boxCollider;

        Vector2 destination;
        int persureStack;

        bool start;

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (!start)
            {
                persureStack = 0;
                destination = new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z);
                start = true;
                if (boxCollider == null)
                {
                    boxCollider = stateMachine.Enemy.GetComponent<BoxCollider>();
                }
            }
            if (start)
            {
                persureStack++;
                if (stateMachine.stateMachineData.destinationUpdateTime <= persureStack)
                {
                    start = false;
                }

                Vector2 dirVec = destination - new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z);

                float distance = Vector2.Distance(new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z));
                SkillSet skillSet = stateMachine.Enemy.GetComponentInChildren<SkillSet>();
                int coolCount = 0;
                foreach (var skillAreaBundle in stateMachine.Enemy.skillArea.skillAreaBundles)
                {
                    if (skillSet.skillStackAmount[skillAreaBundle.eventCategory] < skillSet.skillCoolTimes[skillAreaBundle.eventCategory].Length)
                    {
                        if (skillSet.skillCoolTimes[skillAreaBundle.eventCategory][skillSet.skillStackAmount[skillAreaBundle.eventCategory]] > 0)
                        {
                            coolCount++;
                            continue;
                        }
                    }

                    foreach (var item in skillAreaBundle.skillAreaDatas)
                    {
                        if (item.inLeftSkillArea || item.inRightSkillArea)
                        {
                            float attackQuest = Random.Range(0, 10);
                            if (attackQuest < 9)
                            {
                                return stateMachine.GetStateTable(typeof(Attack));
                            }
                            else
                            {
                                return stateMachine.GetStateTable(typeof(Wait));
                            }
                        }
                    }
                }

                Vector3 pos = stateMachine.Enemy.transform.position + boxCollider.center;
                Vector3 dir = new Vector3(dirVec.x, 0, dirVec.y);
                if (Vector2.Distance(new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z), new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z)) > stateMachine.stateMachineData.searchRadius && Physics.BoxCast(pos - dir.normalized * 0.1f, boxCollider.size / 2, dir, Quaternion.identity, 0.5f, LayerMask.GetMask(new string[] { "Wall" })))
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(Move));
                }

                if (Vector2.Distance(stateMachine.Enemy.spawnPoint, new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) > stateMachine.stateMachineData.activityRadius)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(Move));
                }

                if (dirVec.x > 0)
                {
                    stateMachine.Enemy.direction = true;
                }
                else if (dirVec.x < 0)
                {
                    stateMachine.Enemy.direction = false;
                }
                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, dirVec.x, dirVec.y, stateMachine.Enemy.direction, stateMachine.Enemy.transform.position);

            }
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}
