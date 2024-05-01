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
        public List<CraftingStation> WorkingCraftingStations { get; private set; } = new List<CraftingStation>();
        public CraftingStation TargetedCraftingStation { get; private set; }
        public EnemyWeapon SineGun { get; private set; }
        public EnemyWeapon ShotGun { get; private set; }
        public int TargetedCraftingStationIndex { get; private set; }
        public bool TargetedStationDisabled { get; set; } = false;
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

        private int GetRandomWorkingCraftingStationIndex()
        {
            return Random.Range(0, WorkingCraftingStations.Count);
        }

        private float GetPlayerDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(m_player.transform.position, TargetedCraftingStation.transform.position);
        }

        private float GetDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(transform.position, TargetedCraftingStation.transform.position);
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

        private void VariableSetUp()
        {
            UniqueData = m_characterData as BossEnemyData;

            Agent = m_navMeshAgent;

            Player = m_player;
            
            SineGun = GetComponentInChildren<EnemyWeapon>();
            // TODO Use a GetComponentInChildren when/if implemented
            //ShotGun = GameObject.Find("ShotGun").GetComponent<EnemyWeapon>();

            FindWorkingCraftingStations();
            int nextTargetedCraftingStationIndex = GetRandomWorkingCraftingStationIndex();
            TargetedCraftingStation = WorkingCraftingStations[nextTargetedCraftingStationIndex];
        }

        private void UpdateDistances()
        {
            PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
            PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
            InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;
        }

        public void AttackTargetedCraftingStation()
        {
            TargetedCraftingStation.ReceiveDamage(UniqueData.craftingStationAttackDamage);
            
            Vector2 direction = transform.position - TargetedCraftingStation.transform.position;
            Vector2 directionNorm = direction.normalized;
            Vector2 contactPos = (directionNorm * 5) + new Vector2(TargetedCraftingStation.transform.position.x, TargetedCraftingStation.transform.position.y);  
            
            SpawnContactAttackVFX(contactPos, TargetedCraftingStation.transform);
            
            if(TargetedCraftingStation.GetIsDisabled())
            {
                Debug.Log("Have been disabled"); // À changer d'endroit
                TargetedStationDisabled = true;
            }
        }

        private void FindWorkingCraftingStations()
        {
            WorkingCraftingStations.Clear();
            foreach (CraftingStation station in CraftingStation.GetCraftingStations())
            {
                if (!station.GetIsDisabled())
                {
                    WorkingCraftingStations.Add(station);
                }
            }            
        }
       
    }
}
