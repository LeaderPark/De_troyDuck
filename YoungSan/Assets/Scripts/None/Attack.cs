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
            
            if (Random.Range(0, 100) < 60) return stateMachine.GetStateTable(typeof(SkillCheck));
            
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
                Vector2 position = Vector2.zero;
                if (directions[bundleIdx])
                {
                    int idx = Random.Range(0, bundles[bundleIdx].Item1.RightAreaBox.Length);
                    BoxCollider collider = bundles[bundleIdx].Item1.RightAreaBox[idx].GetComponent<BoxCollider>();
                    position.x = Random.Range(collider.center.x - collider.size.x, collider.center.x + collider.size.x);
                    position.y = Random.Range(collider.center.y - collider.size.y, collider.center.y + collider.size.y);
                }
                else
                {
                    int idx = Random.Range(0, bundles[bundleIdx].Item1.LeftAreaBox.Length);
                    BoxCollider collider = bundles[bundleIdx].Item1.LeftAreaBox[idx].GetComponent<BoxCollider>();
                    position.x = Random.Range(collider.center.x - collider.size.x, collider.center.x + collider.size.x);
                    position.y = Random.Range(collider.center.y - collider.size.y, collider.center.y + collider.size.y);
                }
                UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
                uiManager.EnemyDelayUI.AttackDelayUI();

                stateMachine.Enemy.entityEvent.CallEvent(bundles[bundleIdx].Item2, dirVec.x, dirVec.y, directions[bundleIdx], position);
            }
            catch
            {
                return stateMachine.GetStateTable(typeof(SkillCheck));
            }
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}