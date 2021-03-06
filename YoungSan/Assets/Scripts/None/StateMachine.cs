using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private State state;
        private Hashtable stateTable;
        public Enemy Enemy { get; private set; }

        public StateMachineData stateMachineData;


        public float searchTimeStack { get; set; }

        void Awake()
        {
            Enemy = GetComponent<Enemy>();
            stateTable = new Hashtable();
            stateTable.Add(typeof(Idle), new Idle());
            stateTable.Add(typeof(Move), new Move());
            stateTable.Add(typeof(Pursue), new Pursue());
            stateTable.Add(typeof(SkillCheck), new SkillCheck());
            stateTable.Add(typeof(Attack), new Attack());
            stateTable.Add(typeof(Distance), new Distance());
            stateTable.Add(typeof(Wait), new Wait());
            stateTable.Add(typeof(AttackDelay), new AttackDelay());
            state = GetStateTable(typeof(Idle));
        }

        public static HashSet<Entity> fight = new HashSet<Entity>();

        void Update()
        {
            state = state.Process(this);

            if (fight.Contains(Enemy.entity))
            {
                switch (state.ToString())
                {
                    case "StateMachine.Idle":
                    case "StateMachine.Move":
                        fight.Remove(Enemy.entity);
                        break;
                }
            }
            else
            {
                switch (state.ToString())
                {
                    case "StateMachine.Pursue":
                    case "StateMachine.SkillCheck":
                    case "StateMachine.Attack":
                    case "StateMachine.Distance":
                    case "StateMachine.Wait":
                    case "StateMachine.AttackDelay":
                        fight.Add(Enemy.entity);
                        break;
                }
            }
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
                Gizmos.DrawWireSphere(new Vector3(enemy.spawnPoint.x, 0, enemy.spawnPoint.y), stateMachineData.activityRadius);
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
