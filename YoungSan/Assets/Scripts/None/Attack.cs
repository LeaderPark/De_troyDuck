using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Attack : State
    {
        public override State Process(StateMachine stateMachine)
        {
            List<SkillAreaBundle> bundles = new List<SkillAreaBundle>();
            List<bool> directions = new List<bool>();
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
                stateMachine.Enemy.entityEvent.CallEvent(bundles[bundleIdx].eventCategory, new object[]{0, 0, directions[bundleIdx]});
            }
            catch
            {
                return stateMachine.GetStateTable(typeof(SkillCheck));
            }
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}