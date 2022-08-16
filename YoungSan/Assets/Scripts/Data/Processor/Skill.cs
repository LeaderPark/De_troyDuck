using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Processor
{
    public class Skill : Processor
    {

        SkillSet skillSet;

        public Skill(Hashtable owner, SkillSet skillSet) : base(owner)
        {
            this.skillSet = skillSet;
        }

        private void UseSkill(EventCategory category, int index, Vector2 direction, bool isRight, System.Action action)
        {
            if (Locker) return;
            Entity entity = skillSet.entity;
            if (entity.entityStatusAilment != null)
            {
                if (entity.entityStatusAilment.GetEntityStatus(typeof(Airbone)).Activated()) return;
            }
            skillSet.ActiveSkill(category, index, direction, isRight, action);
        }

        private void Reset()
        {
            skillSet.Reset();
        }

        private void StopSkill()
        {
            skillSet.StopSkill();
        }
    }
}
