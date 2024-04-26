using SpaceBaboon.WeaponSystem;
using SpaceBaboon.Crafting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {
        public BossEnemyData UniqueData { get; private set; }
        public NavMeshAgent Agent { get; set; }
        public Player Player { get; private set; }
        public List<CraftingStation> CraftingStations { get; private set; }
        public EnemyWeapon SineGun { get; private set; }
        public EnemyWeapon ShotGun { get; private set; }
        public int TargetedCraftingStation { get; private set; }           
        public bool PlayerInAggroRange { get; private set; }
        public bool PlayerInTargetedCraftingStationRange { get; private set; }
        public bool InTargetedCraftingStationAttackRange { get; private set; }
        public bool SpecialAttackReady { get; set; } = false;
        public float SpecialAttackTimer { get; set; }    
        public bool ControllerIsActive { get; private set; }        

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
            VariablesAwakeSetUp();
        }

        protected override void Start()
        {
            base.Start();
            StartStates();
            VariablesStartSetUp();
        }

        protected override void Update()
        {
            if (!m_isActive)
                return;

            base.Update();
            UpdateDistances();
            ControllerIsActive = m_isActive;
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
            Agent.SetDestination(value);            
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

        private void VariablesAwakeSetUp()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
        }

        private void VariablesStartSetUp()
        {
            UniqueData = m_characterData as BossEnemyData;
            ControllerIsActive = m_isActive;
            Player = m_player;
            
            SineGun = GetComponentInChildren<EnemyWeapon>();
            // TODO Use a GetComponentInChildren when/if implemented
            //ShotGun = GameObject.Find("ShotGun").GetComponent<EnemyWeapon>();

            CraftingStations = CraftingStation.GetCraftingStations();
            TargetedCraftingStation = GetRandomCraftingStationIndex();
        }

        private void UpdateDistances()
        {
            PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
            PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
            InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;
        }

        public void TemporaryCraftingStationAttack(Vector2 pos)
        {
            InstantiateContactAttackParticuleSystem(pos);
        }

        protected override void SetComponents(bool value)
        {
            base.SetComponents(value);            
            Agent.enabled = value;
        }

        #region Overriden Methods
        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }
        #endregion
    }
}
