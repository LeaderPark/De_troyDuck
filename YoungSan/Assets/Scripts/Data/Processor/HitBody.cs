using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class HitBody : Processor
    {
        private Entity entity;
        public bool isDefencing { private set; get; }
        public float defenceRate { private set; get; }
        public float defendTimer { private set; get; }

        public HitBody(Hashtable owner, Entity entity) : base(owner)
        {
            this.entity = entity;
        }


        public override void Process()
        {
            base.Process();

            if (isDefencing)
            {
                defendTimer -= Time.deltaTime;
                if (defendTimer <= 0)
                {
                    ReleaseDefend();
                }
            }
        }

        void DamageOnBody(int damage, Entity attackEntity)
        {
            GameManager gameManager = ManagerObject.Instance.GetManager(ManagerType.GameManager) as GameManager;
            EventManager eventManager = ManagerObject.Instance.GetManager(ManagerType.EventManager) as EventManager;
            PoolManager poolManager = ManagerObject.Instance.GetManager(ManagerType.PoolManager) as PoolManager;
            UIManager uiManager = ManagerObject.Instance.GetManager(ManagerType.UIManager) as UIManager;
            int oldHealth = entity.clone.GetStat(StatCategory.Health);
            int tempDamage = damage;
            if (isDefencing)
            {
                tempDamage = (int)(tempDamage * (1f - defenceRate));
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

        void Defend(float time, float rate)
        {
            isDefencing = true;
            defenceRate = rate;
            defendTimer = time;
        }

        void ReleaseDefend()
        {
            isDefencing = false;
            defendTimer = 0f;
            defenceRate = 0f;
        }
    }
}