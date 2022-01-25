using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class SkillCheck : State
    {
        public override State Process(StateMachine stateMachine)
        {

            foreach (var skillAreaBundle in stateMachine.Enemy.skillArea.skillAreaBundles)
            {
                foreach (var item in skillAreaBundle.skillAreaDatas)
                {
                    if (item.inLeftSkillArea || item.inRightSkillArea)
                    {
                        if (Vector2.Distance(new Vector2(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) < stateMachine.stateMachineData.distanceRadius)
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

            return stateMachine.GetStateTable(typeof(Pursue));
        }
    }
}
