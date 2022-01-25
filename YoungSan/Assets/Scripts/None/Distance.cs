using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Distance : State
    {
        float moveDirX;
        float moveDirY;

        float moveTime;
        float timeStack;
        bool start;        
        
        public override State Process(StateMachine stateMachine)
        {
            if (!start)
            {
                Vector2 dirVec = new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z) - new Vector2(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.z);
                start = true;
                timeStack = 0;
                moveDirX = Random.Range(-1f, 1f) + dirVec.x;
                moveDirY = Random.Range(-1f, 1f) + dirVec.y;
                moveTime = Random.Range(stateMachine.stateMachineData.minMoveTime, stateMachine.stateMachineData.maxMoveTime);
            }
            if (start)
            {
                timeStack += Time.deltaTime;
                if (Vector2.Distance(stateMachine.Enemy.spawnPoint, (new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) + new Vector2(moveDirX, moveDirY).normalized) < stateMachine.stateMachineData.activityRadius)
                {
                    if (moveDirX > 0)
                    {
                        stateMachine.Enemy.direction = true;
                    }
                    else if (moveDirX < 0)
                    {
                        stateMachine.Enemy.direction = false;
                    }
                    stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, new object[]{moveDirX, moveDirY, stateMachine.Enemy.direction});
                }
                else
                {
                    start = false;
                    timeStack = 0;
                    return stateMachine.GetStateTable(typeof(Move));
                }
                if (moveTime <= timeStack)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(SkillCheck));
                }
            }
            return this;
        }
    }
}
