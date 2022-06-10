using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class AttackDelay : State
    {
        float timeStack;

        bool start;

        SkillSet skillSet;

        public override State Process(StateMachine stateMachine)
        {
            if (skillSet == null) skillSet = stateMachine.Enemy.entity.GetComponentInChildren<SkillSet>();
            if (!start)
            {
                timeStack = 0;
                start = true;
            }
            else
            {
                timeStack += Time.deltaTime;

                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, 0, 0, stateMachine.Enemy.direction, stateMachine.Enemy.transform.position);

                if (timeStack >= skillSet.delayTime)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(SkillCheck));
                }
            }
            return this;
        }
    }
}
