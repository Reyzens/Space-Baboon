using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {
        public BossEnemyData UniqueData { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        [field: SerializeField] public List<GameObject> CraftingStations { get; private set; } //= new List<GameObject>();
        public int TargetedCraftingStation { get; private set; }

        public Player Player { get; private set; }
        public float DistanceToPlayer { get; private set; }
        
        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<BossEnemyState>();
            m_possibleStates.Add(new TestState());            
        }

        protected override void Awake()
        {
            base.Awake();            
        }

        protected override void Start()
        {
            base.Start();
            
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

            Player = m_player;

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
            if (!m_isActive)
                return;

            base.Update();            

            DistanceToPlayer = m_distanceToPlayer;
            
        }

        protected override void FixedUpdate()
        {
            if (!m_isActive)
                return;

            base.FixedUpdate();            
        }

        private int GetRandomCraftingStationIndex()
        {
            return Random.Range(0, CraftingStations.Count);
        }

        public float GetCalculatePlayerDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(Player.transform.position, CraftingStations[TargetedCraftingStation].transform.position);
        }









        protected override void Move(Vector2 value)
        {
            // Overriding this method
        }

        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }


    }
}
