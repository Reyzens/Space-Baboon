using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {
        //private BossEnemyData m_uniqueData;
        public BossEnemyData UniqueData { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        //private NavMeshAgent m_agent;

        [field: SerializeField] public List<GameObject> CraftingStations { get; private set; } //= new List<GameObject>();
        //[SerializeField] 
        public int TargetedCraftingStation { get; private set; }

        public Player PlayerYo { get; private set; }
        public float DistanceToPlayer { get; private set; }


        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<BossEnemyState>();
            m_possibleStates.Add(new TestState());            
        }

        protected override void Awake()
        {
            base.Awake();
            //BaseEnemyStateMachineAwake();
        }

        protected override void Start()
        {
            base.Start();
            //BaseEnemyStateMachineStart();
            
            foreach (BossEnemyState state in m_possibleStates)
            {
                state.OnStart(this);
            }

            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();

            //////////////
            UniqueData = m_characterData as BossEnemyData;

            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;

            PlayerYo = m_player;

            // TODO Remove asap when we have references to stations from manager
            CraftingStations.Add(GameObject.Find("CraftingStationOne"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (1)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (2)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (3)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (4)"));

            TargetedCraftingStation = GetRandomCraftingStationIndex();
        }

        protected override void Update()
        {
            base.Update();
            BaseEnemyStateMachineUpdate();

            DistanceToPlayer = m_distanceToPlayer;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            BaseEnemyStateMachineFixedUpdate();
        }

        private int GetRandomCraftingStationIndex()
        {
            return Random.Range(0, CraftingStations.Count);
        }

        protected override void Move(Vector2 value)
        {
            // Overriden, not used
            //m_agent.SetDestination(value);
        }

        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }


    }
}
