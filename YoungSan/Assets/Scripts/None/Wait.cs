using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Wait : State
    {
        public override State Process(StateMachine stateMachine)
        {
            return this;
        }
    }
}
