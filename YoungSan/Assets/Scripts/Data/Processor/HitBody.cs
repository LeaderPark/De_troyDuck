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
            entity.clone.SubStat(StatCategory.Health, damage);
            if ((int)entity.clone.GetStat(StatCategory.Health) - damage <= 0)
            {
                entity.Die();
            }
            Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", entity.clone.Name, entity.clone.GetStat(StatCategory.Health), entity.clone.GetStat(StatCategory.Attack), entity.clone.GetStat(StatCategory.Speed), entity.clone.GetStat(StatCategory.Stamina)));
        }
    }
}