using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class SkillCheck : State
    {
        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            float distance = Vector2.Distance(new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z));
            foreach (var skillAreaBundle in stateMachine.Enemy.skillArea.skillAreaBundles)
            {
                bool b = false;
                foreach (var item in stateMachine.Enemy.GetComponentInChildren<SkillSet>().skillCools[skillAreaBundle.eventCategory])
                {
                    if (item)
                    {
                        b = true;
                    }
                }
                if (b) continue;
                foreach (var item in skillAreaBundle.skillAreaDatas)
                {
                    if (item.inLeftSkillArea || item.inRightSkillArea)
                    {
                        if (distance < stateMachine.stateMachineData.distanceRadius)
                        {
                            return stateMachine.GetStateTable(typeof(Distance));
                        }
                        float attackQuest = Random.Range(0, 10);
                        if (attackQuest < 8)
                        {
                            return stateMachine.GetStateTable(typeof(Attack));
                        }
                        else
                        {
                            return stateMachine.GetStateTable(typeof(Distance));
                        }
                    }
                }
            }
            
            
            if (distance < stateMachine.stateMachineData.distanceRadius)
            {
                return stateMachine.GetStateTable(typeof(Distance));
            }
            else
            {
                float pursueQuest = Random.Range(0, 10);
                if (pursueQuest < 9)
                {
                    return stateMachine.GetStateTable(typeof(Pursue));
                }
                else
                {
                    return stateMachine.GetStateTable(typeof(Wait));
                }
            }
        }
    }
}
