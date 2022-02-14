using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Wait : State
    {
        float timeStack;

        bool moveDir;

        bool start;

        public override State Process(StateMachine stateMachine)
        {
            if (!start)
            {
                timeStack = 0;
                moveDir = Random.Range(0, 2) == 0;
                start = true;
            }
            if (start)
            {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
                timeStack += Time.deltaTime;
                
                Vector3 dirVec = stateMachine.Enemy.transform.position - gameManager.Player.transform.position;
                dirVec.y = 0;
                
                dirVec = Quaternion.AngleAxis(90, Vector3.up) * dirVec;
                if (moveDir) dirVec *= -1;

                if (dirVec.x > 0f)
                {
                    stateMachine.Enemy.direction = true;
                }
                else
                {
                    stateMachine.Enemy.direction = false;
                }

                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, new object[]{dirVec.x, dirVec.z, stateMachine.Enemy.direction});

                if (stateMachine.stateMachineData.waitTime <= timeStack)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(SkillCheck));
                }
            }
            return this;
        }
    }
}
