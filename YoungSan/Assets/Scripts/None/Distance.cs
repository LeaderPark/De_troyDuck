using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Distance : State
    {
        public override State Process(StateMachine stateMachine)
        {
            return stateMachine.GetStateTable(typeof(SkillCheck));
        }
    }
}
