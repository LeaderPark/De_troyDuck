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

        private void UseSkill(int index, Vector2 direction, bool isRight, System.Action action)
        {
            lock(lockObject)
            {
                if (Locker) return;
            }
            skillSet.ActiveSkill(index, direction, isRight, action);
        }

        private void StopSkill()
        {
            skillSet.StopSkill();
        }
    }
}
