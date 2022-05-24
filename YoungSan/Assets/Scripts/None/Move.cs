using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Move : State
    {
        float moveDirX;
        float moveDirY;

        float moveTime;
        float timeStack;
        bool start;

        bool backSpawnPoint;

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (backSpawnPoint)
            {
                timeStack += Time.deltaTime;
                Vector2 dirVec = (Vector2)stateMachine.Enemy.spawnPoint - new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z);
                
                if (dirVec.x > 0)
                {
                    stateMachine.Enemy.direction = true;
                }
                else if (dirVec.x < 0)
                {
                    stateMachine.Enemy.direction = false;
                }
                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, dirVec.x, dirVec.y, stateMachine.Enemy.direction, stateMachine.Enemy.transform.position);
                if (Vector2.Distance(stateMachine.Enemy.spawnPoint, new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) < (stateMachine.stateMachineData.activityRadius - stateMachine.stateMachineData.searchDelay) / 4f
                || stateMachine.stateMachineData.homeTime <= timeStack)
                {
                    backSpawnPoint = false;
                    start = false;
                }
                return this;
            } 

            if (Vector2.Distance(new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) <= stateMachine.stateMachineData.searchRadius)
            {
                stateMachine.searchTimeStack += Time.deltaTime;
            }
            else
            {
                stateMachine.searchTimeStack = 0;
            }
            if (!gameManager.Player.GetComponent<Entity>().isDead)
            {
                if (stateMachine.stateMachineData.searchDelay <= stateMachine.searchTimeStack)
                {
                    start = false;
                    stateMachine.searchTimeStack = 0;
                    return stateMachine.GetStateTable(typeof(Pursue));
                }
            }
            if (!start)
            {
                start = true;
                timeStack = 0;
                moveDirX = Random.Range(-1f, 1f);
                moveDirY = Random.Range(-1f, 1f);
                moveTime = Random.Range(stateMachine.stateMachineData.minMoveTime, stateMachine.stateMachineData.maxMoveTime);
            }
            if (start)
            {
                timeStack += Time.deltaTime;
                if (moveTime <= timeStack)
                {
                    start = false;
                    float idleQuest = Random.Range(0, 10);
                    if (idleQuest < 5)
                    {
                        return stateMachine.GetStateTable(typeof(Idle));
                    }
                }
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
                    stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, moveDirX, moveDirY, stateMachine.Enemy.direction, stateMachine.Enemy.transform.position);
                }
                else
                {
                    backSpawnPoint = true;
                    timeStack = 0;
                }
            }
            return this;
        }
    }
}
