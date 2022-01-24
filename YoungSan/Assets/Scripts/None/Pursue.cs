using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Pursue : State
    {
        BoxCollider boxCollider;
        bool start;
        public override State Process(StateMachine stateMachine)
        {
            if (!start)
            {
                start = true;
                if (boxCollider == null)
                {
                    boxCollider = stateMachine.Enemy.GetComponent<BoxCollider>();
                }
            }
            if (start)
            {
                Vector2 dirVec = new Vector2(stateMachine.Player.transform.position.x, stateMachine.Player.transform.position.z) - new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z);
                Vector3 pos = stateMachine.Enemy.transform.position + boxCollider.center;
                Vector3 dir = new Vector3(dirVec.x, 0, dirVec.y);
                if (Physics.BoxCast(pos - dir.normalized * 0.1f, boxCollider.size / 2, dir, Quaternion.identity, 0.5f, LayerMask.GetMask(new string[]{"Wall"})))
                {
                    return stateMachine.GetStateTable(typeof(Move));
                }

                if (Vector2.Distance(stateMachine.Enemy.spawnPoint, new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z)) > stateMachine.stateMachineData.activityRadius)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(Move));
                }
                
                if (dirVec.x > 0)
                {
                    stateMachine.Enemy.direction = true;
                }
                else if (dirVec.x < 0)
                {
                    stateMachine.Enemy.direction = false;
                }
                stateMachine.Enemy.entityEvent.CallEvent(EventCategory.Move, new object[]{dirVec.x, dirVec.y, stateMachine.Enemy.direction});
            }
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}
