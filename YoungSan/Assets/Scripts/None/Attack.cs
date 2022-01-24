using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Attack : State
    {
        public override State Process(StateMachine stateMachine)
        {
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}