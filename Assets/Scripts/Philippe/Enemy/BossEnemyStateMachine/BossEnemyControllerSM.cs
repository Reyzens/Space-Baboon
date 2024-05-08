using SpaceBaboon.WeaponSystem;
using SpaceBaboon.Crafting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SpaceBaboon.PoolingSystem;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemyControllerSM : BaseEnemyStateMachine<BossEnemyState>
    {                        
        public BossEnemyData UniqueData { get; private set; }
        public NavMeshAgent Agent { get; set; }
        public Animator Animator { get; set; }
        public Player Player { get; private set; }     
        public EnemySpawner EnemySpawner { get; private set; }
        public List<CraftingStation> WorkingCraftingStations { get; private set; } = new List<CraftingStation>();
        public CraftingStation TargetedCraftingStation { get; private set; }
        public EnemyWeapon SineGun { get; private set; }
        //public EnemyWeapon ShotGun { get; private set; }        
        public bool TargetedStationDisabled { get; set; } = false;
        public bool PlayerInAggroRange { get; private set; }
        public bool PlayerInTargetedCraftingStationRange { get; private set; }
        public bool InTargetedCraftingStationAttackRange { get; private set; }
        public bool SpecialAttackReady { get; set; } = false;
        public float SpecialAttackTimer { get; set; }
        public int CurrentBossIndex { get; private set; }

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
            UniqueData = m_characterData as BossEnemyData;            
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

            if(Input.GetKeyDown(KeyCode.B)) //TODO to remove, for testing purposes
            {
                OnDamageTaken(1000);
            }

            base.Update();
            UpdateDistances();
        }

        protected override void FixedUpdate()
        {
            if (!m_isActive)
                return;

            base.FixedUpdate();            
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
            Agent = m_navMeshAgent;
            Player = m_player;
            EnemySpawner = m_enemySpawner;
            Animator = m_animator;

            SineGun = GetComponentInChildren<EnemyWeapon>();
            // TODO Use a GetComponentInChildren when/if implemented
            //ShotGun = GameObject.Find("ShotGun").GetComponent<EnemyWeapon>();

            TargetRandomWorkingCraftingStation();
        }

        public new void Move(Vector2 value)
        {
            Agent.SetDestination(value);
        }        
        
        private void UpdateDistances()
        {
            PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
            PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
            InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;
        }

        public void AttackTargetedCraftingStation()
        {
            //TODO maybe change FX to an explosion or something
            TargetedCraftingStation.ReceiveDamage(UniqueData.craftingStationAttackDamage);
            SpawnFX();
            CheckIfTargetedCraftinStationIsDisabled();
        }

        private void SpawnFX()
        {
            Vector2 directionNorm = (transform.position - TargetedCraftingStation.transform.position).normalized;
            Vector2 contactPos = (directionNorm * UniqueData.craftingStationAttackFXDistanceThreshold) + new Vector2(TargetedCraftingStation.transform.position.x, TargetedCraftingStation.transform.position.y);
            SpawnContactAttackVFX(contactPos, TargetedCraftingStation.transform);
        }

        private void CheckIfTargetedCraftinStationIsDisabled()
        {
            if (TargetedCraftingStation.GetIsDisabled())
            {
                //Debug.Log("Have been disabled");
                TargetedStationDisabled = true;
                TargetRandomWorkingCraftingStation();
            }
        }

        private void FindWorkingCraftingStations()
        {
            WorkingCraftingStations.Clear();
            foreach (CraftingStation station in CraftingStation.GetCraftingStations())
            {
                if (!station.GetIsDisabled())
                    WorkingCraftingStations.Add(station);
            }            
        }

        private void TargetRandomWorkingCraftingStation()
        {
            FindWorkingCraftingStations();
            int nextTargetedCraftingStationIndex = GetRandomWorkingCraftingStationIndex();
            TargetedCraftingStation = WorkingCraftingStations[nextTargetedCraftingStationIndex];
        }

        private int GetRandomWorkingCraftingStationIndex() { return Random.Range(0, WorkingCraftingStations.Count); }
        private float GetPlayerDistanceToTargetedCraftingStation() { return Vector3.Distance(m_player.transform.position, TargetedCraftingStation.transform.position); }
        private float GetDistanceToTargetedCraftingStation() { return Vector3.Distance(transform.position, TargetedCraftingStation.transform.position); }

        private void SetToRandomBossType()
        {
            CurrentBossIndex = Random.Range(0, (int)EBossTypes.Count);            
          
            m_renderer.color = UniqueData.bosses[CurrentBossIndex].color;
            m_animator.runtimeAnimatorController = UniqueData.bosses[CurrentBossIndex].animControllerObject.GetComponent<Animator>().runtimeAnimatorController;
        }

        public override void Activate(Vector2 pos, GenericObjectPool pool)
        {            
            ResetValues(pos);
            SetComponents(true);
            m_parentPool = pool;
            SetToRandomBossType();
        }
    }
}
