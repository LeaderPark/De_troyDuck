using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Distance : State
    {
        float moveDirX;
        float moveDirY;

        bool start;        
        
        public override State Process(StateMachine stateMachine)
        {
            Vector2 dirVec = new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z) - new Vector2(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.z);
            moveDirX = Random.Range(-1f, 1f) + dirVec.x;
            moveDirY = Random.Range(-1f, 1f) + dirVec.y;
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
                return stateMachine.GetStateTable(typeof(Move));
            }
            
            if (Vector2.Distance(new Vector2(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) < stateMachine.stateMachineData.distanceRadius)
            {
                return stateMachine.GetStateTable(typeof(Distance));
            }
            else
            {
                return stateMachine.GetStateTable(typeof(SkillCheck));
            }
        }
    }
}
