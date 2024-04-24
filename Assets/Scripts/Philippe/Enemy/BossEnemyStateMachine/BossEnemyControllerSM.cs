using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {
        public BossEnemyData UniqueData { get; private set; } // TODO maybe remove field
        public NavMeshAgent Agent { get; private set; } // TODO maybe remove field

        [field: SerializeField] public List<GameObject> CraftingStations { get; private set; } // TODO maybe remove field //= new List<GameObject>();
        public int TargetedCraftingStation { get; private set; } // TODO maybe remove field

        public Player Player { get; private set; } // TODO maybe remove field
        public float DistanceToPlayer { get; private set; }
        public bool PlayerInAggroRange { get; private set; }
        public bool PlayerInTargetedCraftingStationRange { get; private set; }
        public bool InTargetedCraftingStationAttackRange { get; private set; }
        public bool SpecialAttackReady { get; set; } = false;
        public float SpecialAttackTimer { get; set; }
        
        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<BossEnemyState>();            
            m_possibleStates.Add(new MovingToStation());
            m_possibleStates.Add(new ChasingPlayer());
            m_possibleStates.Add(new DoSpecialAttack());
            m_possibleStates.Add(new AttackingStation());            
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
                        
            PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
            PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
            InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;                       
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

        private float GetPlayerDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(Player.transform.position, CraftingStations[TargetedCraftingStation].transform.position);
        }

        private float GetDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(transform.position, CraftingStations[TargetedCraftingStation].transform.position);
        }

        public new void Move(Vector2 value)
        {
            Agent.SetDestination(value);            
        }






        #region Overriden Methods


        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }
        #endregion
    }
}
