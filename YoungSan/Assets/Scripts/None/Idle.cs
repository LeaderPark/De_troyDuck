using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Idle : State
    {
        float idleTime;
        float timeStack;
        bool start;

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            if (Vector2.Distance(new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z), new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) <= stateMachine.stateMachineData.searchRadius)
            {
                stateMachine.searchTimeStack += Time.deltaTime;
            }
            else
            {
                stateMachine.searchTimeStack = 0;
            }
            if (stateMachine.stateMachineData.searchDelay <= stateMachine.searchTimeStack)
            {
                start = false;
                stateMachine.searchTimeStack = 0;
                return stateMachine.GetStateTable(typeof(Pursue));
            }
            if (!start)
            {
                start = true;
                timeStack = 0;
                idleTime = Random.Range(stateMachine.stateMachineData.minIdleTime, stateMachine.stateMachineData.maxIdleTime);
            }
            if (start)
            {
                timeStack += Time.deltaTime;
                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, new object[]{0, 0, stateMachine.Enemy.direction});
                if (idleTime <= timeStack)
                {
                    start = false;

                    float moveQuest = Random.Range(0, 10);
                    if (moveQuest < 5)
                    {
                        return stateMachine.GetStateTable(typeof(Move));
                    }
                }
            }
            return this;
        }
    }
}
