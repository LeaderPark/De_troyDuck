using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class HitBody : Processor
    {
        private Entity entity;

        public HitBody(Hashtable owner, Entity entity) : base(owner)
        {
            this.entity = entity;
        }


        public override void Process()
        {
            base.Process();
        }

        void DamageOnBody(int damage, Entity attackEntity)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            int oldHealth = entity.clone.GetStat(StatCategory.Health);
            int tempDamage = damage;

            if (entity.gameObject.layer == 7)
            {
                entity.GetComponent<StateMachine.StateMachine>().SetState(typeof(StateMachine.Pursue));
            }

            Defending defending = entity.entityStatusAilment.GetEntityStatus(typeof(Defending)) as Defending;
            if (defending.Activated())
            {
                tempDamage = defending.GetData(entity, damage);
            }

            uiManager.damageCountUI.Play(entity.transform.position + Vector3.up * entity.entityData.uiPos * 0.5f, tempDamage, (entity == gameManager.Player.GetComponent<Entity>()) ? true : false, false);

            eventManager.GetEventTrigger(typeof(HitEventTrigger)).Invoke(new object[] { entity, attackEntity, tempDamage });

            entity.clone.SubStat(StatCategory.Health, tempDamage);
            if (oldHealth - tempDamage <= 0)
            {
                eventManager.GetEventTrigger(typeof(DieEventTrigger)).Invoke(new object[] { entity, attackEntity });
                entity.Die();
            }
            Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", entity.clone.Name, entity.clone.GetStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Attack), entity.clone.GetStat(StatCategory.Speed), entity.clone.GetStat(StatCategory.Stamina)));
        }
    }
}