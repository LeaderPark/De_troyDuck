using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Attack : State
    {
        List<SkillAreaBundle> bundles = new List<SkillAreaBundle>();
        List<bool> directions = new List<bool>();

        float timeStack;
        bool start;

        public override State Process(StateMachine stateMachine)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            
            if (Random.Range(0, 100) < 60) return stateMachine.GetStateTable(typeof(SkillCheck));
            
            if (!start)
            {
                timeStack = 0;
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
                }
                start = true;
            }
            if (start)
            {
                timeStack += Time.deltaTime;
                
                if (timeStack >= 0.5f)
                {
                    start = false;
                    return stateMachine.GetStateTable(typeof(SkillCheck));
                }
                return this;
            }

            return this;
        }
    }
}