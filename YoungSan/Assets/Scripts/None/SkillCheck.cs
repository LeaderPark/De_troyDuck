using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class SkillCheck : State
    {
        public override State Process(StateMachine stateMachine)
        {

            foreach (var item in stateMachine.Enemy.skillArea.skillAreaDatas)
            {
                if (item.inLeftSkillArea || item.inRightSkillArea)
                {
                    float attackQuest = Random.Range(0, 10);
                    if (attackQuest < 5)
                    {
                        return stateMachine.GetStateTable(typeof(Attack));
                    }
                    else
                    {
                        return stateMachine.GetStateTable(typeof(Distance));
                    }
                }
            }

            return stateMachine.GetStateTable(typeof(Pursue));
        }
    }
}
