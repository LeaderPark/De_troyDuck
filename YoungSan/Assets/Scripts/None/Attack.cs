using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Attack : State
    {
        List<(SkillAreaData, EventCategory)> bundles = new List<(SkillAreaData, EventCategory)>();
        List<bool> directions = new List<bool>();

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;

            bundles.Clear();
            directions.Clear();

            foreach (var skillAreaBundle in stateMachine.Enemy.skillArea.skillAreaBundles)
            {
                foreach (var item in skillAreaBundle.skillAreaDatas)
                {
                    if (item.inLeftSkillArea)
                    {
                        bundles.Add((item, skillAreaBundle.eventCategory));
                        directions.Add(false);
                    }
                    if (item.inRightSkillArea)
                    {
                        bundles.Add((item, skillAreaBundle.eventCategory));
                        directions.Add(true);
                    }
                }
            }

            int bundleIdx = Random.Range(0, bundles.Count);

            try
            {
                Vector2 dirVec = new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z) - new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z);
                Vector3 position = gameManager.Player.transform.position;

                stateMachine.Enemy.direction = directions[bundleIdx];
                stateMachine.Enemy.entityEvent.CallEvent(bundles[bundleIdx].Item2, dirVec.x, dirVec.y, directions[bundleIdx], position);
            }
            catch
            {
                return stateMachine.GetStateTable(typeof(AttackDelay));
            }
            return stateMachine.GetStateTable(typeof(AttackDelay));
        }
    }
}