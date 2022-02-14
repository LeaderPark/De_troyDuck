using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Attack : State
    {
        List<SkillAreaBundle> bundles = new List<SkillAreaBundle>();
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
                    bundles.Add(skillAreaBundle);
                    if (item.inLeftSkillArea)
                    {
                        directions.Add(false);
                    }
                    if (item.inRightSkillArea)
                    {
                        directions.Add(true);
                    }
                }
            }
            int bundleIdx = Random.Range(0, bundles.Count);
            
            try
            {
                Vector2 dirVec = new Vector2(gameManager.Player.transform.position.x, gameManager.Player.transform.position.z) - new Vector2(stateMachine.Enemy.transform.position.x, stateMachine.Enemy.transform.position.z);
                stateMachine.Enemy.entityEvent.CallEvent(bundles[bundleIdx].eventCategory, new object[]{dirVec.x, dirVec.y, directions[bundleIdx]});
            }
            catch
            {
                return stateMachine.GetStateTable(typeof(SkillCheck));
            }
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}