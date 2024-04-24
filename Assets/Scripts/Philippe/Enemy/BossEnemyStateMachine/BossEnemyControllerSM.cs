using SpaceBaboon.WeaponSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {
        public BossEnemyData UniqueData { get; private set; }
        public Player Player { get; private set; }
        [field: SerializeField] public List<GameObject> CraftingStations { get; private set; }
        public EnemyWeapon SineGun { get; private set; }
        public EnemyWeapon ShotGun { get; private set; }
        public int TargetedCraftingStation { get; private set; }           
        public bool PlayerInAggroRange { get; private set; }
        public bool PlayerInTargetedCraftingStationRange { get; private set; }
        public bool InTargetedCraftingStationAttackRange { get; private set; }
        public bool SpecialAttackReady { get; set; } = false;
        public float SpecialAttackTimer { get; set; }        
        
        private NavMeshAgent m_agent;

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
            StartStates();
            VariableSetUp();
        }

        protected override void Update()
        {
            if (!m_isActive)
                return;

            base.Update();
            UpdateDistances();
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
            return Vector3.Distance(m_player.transform.position, CraftingStations[TargetedCraftingStation].transform.position);
        }

        private float GetDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(transform.position, CraftingStations[TargetedCraftingStation].transform.position);
        }

        public new void Move(Vector2 value)
        {
            m_agent.SetDestination(value);            
        }

        private void StartStates()
        {
            foreach (BossEnemyState state in m_possibleStates)
            {
                state.OnStart(this);
            }

            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();
        }

        private void VariableSetUp()
        {
            UniqueData = m_characterData as BossEnemyData;

            m_agent = GetComponent<NavMeshAgent>();
            m_agent.updateRotation = false;
            m_agent.updateUpAxis = false;

            Player = m_player;

            SineGun = GameObject.Find("SineGun").GetComponent<EnemyWeapon>();
            ShotGun = GameObject.Find("ShotGun").GetComponent<EnemyWeapon>();

            // TODO Remove asap when we have references to stations from manager
            CraftingStations.Add(GameObject.Find("CraftingStationOne"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (1)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (2)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (3)"));
            CraftingStations.Add(GameObject.Find("CraftingStationOne (4)"));

            TargetedCraftingStation = GetRandomCraftingStationIndex();
        }

        private void UpdateDistances()
        {
            PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
            PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
            InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;
        }

        #region Overriden Methods
        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }
        #endregion
    }
}
