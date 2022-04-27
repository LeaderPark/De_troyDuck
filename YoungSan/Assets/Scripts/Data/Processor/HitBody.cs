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

        void DamageOnBody(int damage, Entity attackEntity)
        {
            EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
            int oldHealth = entity.clone.GetStat(StatCategory.Health);
            
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;

            DamageCount damageCount = poolManager.GetObject("DamageCount").GetComponent<DamageCount>();
            damageCount.Play(entity.transform.position + Vector3.up * 0.5f, damage);

            eventManager.GetEventTrigger(typeof(HitEventTrigger)).Invoke(new object[]{ entity, attackEntity, damage });

            entity.clone.SubStat(StatCategory.Health, damage);
            if (oldHealth - damage <= 0)
            {
                eventManager.GetEventTrigger(typeof(DieEventTrigger)).Invoke(new object[]{ entity, attackEntity });
                entity.Die();
            }
            Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", entity.clone.Name, entity.clone.GetStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Attack), entity.clone.GetStat(StatCategory.Speed), entity.clone.GetStat(StatCategory.Stamina)));
        }
    }
}