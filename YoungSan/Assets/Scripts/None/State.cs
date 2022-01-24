using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public abstract class State
    {
        public abstract State Process(StateMachine stateMachine);
    }
}