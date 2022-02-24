using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class HitBody : Processor
    {
        private Clone clone;

        public HitBody(Hashtable owner, Clone clone) : base(owner)
        {
            this.clone = clone;
        }

        void DamageOnBody(int damage, Entity attackEntity)
        {
            clone.SubStat(StatCategory.Health, damage);
            Debug.Log(string.Format("Name : {0}, HP : {1}, Atk : {2}, Speed : {3}, Stamina : {4}", clone.Name, clone.GetStat(StatCategory.Health), clone.GetStat(StatCategory.Attack), clone.GetStat(StatCategory.Speed), clone.GetStat(StatCategory.Stamina)));
        }
    }
}