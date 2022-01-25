using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private State state;
        private Hashtable stateTable;
        public Enemy Enemy {get; private set;}
        public Player Player {get; private set;}
        
        public StateMachineData stateMachineData;
        

        public float searchTimeStack {get; set;}

        void Awake()
        {
            Enemy = GetComponent<Enemy>();
            Player = GameObject.FindObjectOfType<Player>();
            stateTable = new Hashtable();
            stateTable.Add(typeof(Idle), new Idle());
            stateTable.Add(typeof(Move), new Move());
            stateTable.Add(typeof(Pursue), new Pursue());
            stateTable.Add(typeof(SkillCheck), new SkillCheck());
            stateTable.Add(typeof(Attack), new Attack());
            stateTable.Add(typeof(Distance), new Distance());
            state = GetStateTable(typeof(Idle));
        }

        void Update()
        {
            state = state.Process(this);
            Debug.Log(state.GetType().Name);
        }

        public State GetStateTable(System.Type type)
        {
            if (stateTable.ContainsKey(type))
            {
                return (State)stateTable[type];
            }
            return null;
        }

        void OnDrawGizmos()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            Enemy enemy = GetComponent<Enemy>();
            Vector3 pos = enemy.transform.position;
            pos.y = 0;
            Vector2 dirVec = new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            Gizmos.DrawRay(new Ray(pos, new Vector3(dirVec.x, 0, dirVec.y)));
        }
    }
}
