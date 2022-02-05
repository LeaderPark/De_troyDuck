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
            stateTable.Add(typeof(Wait), new Wait());
            state = GetStateTable(typeof(Idle));
        }

        void Update()
        {
            if (Player == null) Player = GameObject.FindObjectOfType<Player>();
            if (Player == null) return;
            state = state.Process(this);
            Debug.Log(state.GetType().Name);
        }

        public void SetState(System.Type type)
        {
            state = GetStateTable(type);
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
            if (player != null)
            {
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                {
                    Vector3 pos = enemy.transform.position;
                    pos.y = 0;
                    Vector2 dirVec = new Vector2(player.transform.position.x, player.transform.position.z) - new Vector2(enemy.transform.position.x, enemy.transform.position.z);
                    Gizmos.DrawRay(new Ray(pos, new Vector3(dirVec.x, 0, dirVec.y)));
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Enemy enemy = GetComponent<Enemy>();
            if (enemy != null && stateMachineData != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(enemy.spawnPoint, stateMachineData.activityRadius);
                Gizmos.color = Color.blue;
                Vector3 temp = transform.position;
                temp.y = 0;
                Gizmos.DrawWireSphere(temp, stateMachineData.searchRadius);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(temp, stateMachineData.distanceRadius);
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(temp, stateMachineData.destinationRadius);
            }
        }
    }
}
