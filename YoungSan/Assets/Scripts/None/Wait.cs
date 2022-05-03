using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Wait : State
    {
        float timeStack;

        int moveDir;

        bool start;

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (!start)
            {
                timeStack = 0;
                moveDir = Random.Range(0, 5);
                start = true;
            }
            else
            {
                timeStack += Time.deltaTime;
                
                if (moveDir > 1)
                {
                    if (stateMachine.stateMachineData.waitTime + stateMachine.stateMachineData.stopTime <= timeStack)
                    {
                        start = false;
                        return stateMachine.GetStateTable(typeof(SkillCheck));
                    }
                }
                else
                {
                    if (stateMachine.stateMachineData.waitTime <= timeStack)
                    {
                        start = false;
                        return stateMachine.GetStateTable(typeof(SkillCheck));
                    }
                }
                
                Vector3 dirVec = stateMachine.Enemy.transform.position - gameManager.Player.transform.position;
                dirVec.y = 0;
                
                dirVec = Quaternion.AngleAxis(90, Vector3.up) * dirVec;
                if (moveDir == 1) dirVec *= -1;
                if (moveDir > 1) dirVec *= 0;

                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, dirVec.x, dirVec.z, stateMachine.Enemy.direction, stateMachine.Enemy.transform.position);
            }
            return this;
        }
    }
}
