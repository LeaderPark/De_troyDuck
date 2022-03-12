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
            skillSet.ActiveSkill(category, index, direction, isRight, action);
        }

        private void StopSkill()
        {
            skillSet.StopSkill();
        }
    }
}
