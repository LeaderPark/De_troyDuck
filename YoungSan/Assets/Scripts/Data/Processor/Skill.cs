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

        private void UseSkill(int index, bool isRight, System.Action action)
        {
            skillSet.ActiveSkill(index, isRight, action);
        }
    }
}
