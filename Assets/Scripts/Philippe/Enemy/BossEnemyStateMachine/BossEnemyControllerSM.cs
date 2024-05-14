using SpaceBaboon.WeaponSystem;
using SpaceBaboon.Crafting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SpaceBaboon.PoolingSystem;
using UnityEngine.Rendering;
using SpaceBaboon.CollisionSystem;

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
        public CraftingStation TargetedCraftingStation { get; private set; } = null;
        public EnemyWeapon SineGun { get; private set; }
        //public EnemyWeapon ShotGun { get; private set; }        
        public bool TargetedStationDisabled { get; set; } = false;        
        public bool StationAvailableToTarget { get; private set; } = false;
        public bool PlayerInAggroRange { get; private set; }
        public bool PlayerInTargetedCraftingStationRange { get; private set; }
        public bool InTargetedCraftingStationAttackRange { get; private set; }
        public bool SpecialAttackReady { get; set; } = false;
        public float SpecialAttackTimer { get; set; }
        public float DistanceToPlayer { get; private set; }
        public int CurrentBossIndex { get; private set; }
         
        private Hitbox m_hitbox;
        private float m_dyingAnimDelay = 5.0f;
        private float m_dyingAnimTimer = 0.0f;

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
            VariablesSetUp();
        }

        protected override void Update()
        {
            if (!m_isActive)
                return;

            if(Input.GetKeyDown(KeyCode.B)) //TODO to remove, for testing purposes
            {
                OnDamageTaken(1000);
            }

            if(m_dyingAnimTimer > 0)
            {
                DoBossDyingAnimAndUnspawn();
                return;
            }

            base.Update();            

            CheckForCraftingStationToAttackElseAttackPlayer();
            UpdateDistances();
            UpdateDistancesBools();
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

        private void VariablesSetUp()
        {
            m_hitbox = GetComponent<Hitbox>();
            Agent = m_navMeshAgent;
            Player = m_player;
            EnemySpawner = m_enemySpawner;
            Animator = m_animator;

            SineGun = GetComponentInChildren<EnemyWeapon>();
            // TODO Use a GetComponentInChildren when/if implemented
            //ShotGun = GameObject.Find("ShotGun").GetComponent<EnemyWeapon>();

            PlayerInAggroRange = false;
            PlayerInTargetedCraftingStationRange = false;
            InTargetedCraftingStationAttackRange = false;
            
            TargetedCraftingStation = null;            
        }

        public new void Move(Vector2 value)
        {
            Agent.SetDestination(value);
        }        
        
        private void UpdateDistancesBools()
        {
            if (StationAvailableToTarget) 
            {
                PlayerInAggroRange = m_distanceToPlayer < UniqueData.playerAggroRange;
                PlayerInTargetedCraftingStationRange = GetPlayerDistanceToTargetedCraftingStation() < UniqueData.possibleAggroRange;
                InTargetedCraftingStationAttackRange = GetDistanceToTargetedCraftingStation() < UniqueData.craftingStationAttackRange;
            }            
        }

        private void UpdateDistances()
        {
            DistanceToPlayer = m_distanceToPlayer;
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

        private void CheckForCraftingStationToAttackElseAttackPlayer()
        {
            bool workingCraftingStationPresent = false;

            workingCraftingStationPresent = FindWorkingCraftingStations();

            if (workingCraftingStationPresent) 
            {
                StationAvailableToTarget = true;
                if (TargetedCraftingStation == null)
                {
                    TargetRandomWorkingCraftingStation();
                } 
            }
            else
            {
                StationAvailableToTarget = false;
                PlayerInAggroRange = false;
                PlayerInTargetedCraftingStationRange = false;
                InTargetedCraftingStationAttackRange = false;
            }
        }

        private bool FindWorkingCraftingStations()
        {
            WorkingCraftingStations.Clear();
            
            foreach (CraftingStation station in CraftingStation.GetCraftingStations())
            {
                if (!station.GetIsDisabled())
                    WorkingCraftingStations.Add(station);
            }
            
            if (WorkingCraftingStations.Count == 0)
            {                
                //Debug.Log("No working crafting station found");
                return false;                
            }

            return true;
        }

        private void TargetRandomWorkingCraftingStation()
        {
            //Debug.Log("WorkingCraftingStationsCount is " + WorkingCraftingStations.Count);
            int nextTargetedCraftingStationIndex = GetRandomWorkingCraftingStationIndex();
            //Debug.Log("Next targeted Crafting Station index is " + nextTargetedCraftingStationIndex);
            TargetedCraftingStation = WorkingCraftingStations[nextTargetedCraftingStationIndex];
        }

        private int GetRandomWorkingCraftingStationIndex() { return Random.Range(0, WorkingCraftingStations.Count); }
        private float GetPlayerDistanceToTargetedCraftingStation() { return Vector3.Distance(m_player.transform.position, TargetedCraftingStation.transform.position); }
        public float GetDistanceToTargetedCraftingStation() { return Vector3.Distance(transform.position, TargetedCraftingStation.transform.position); }

        private void SetToRandomBossType()
        {
            CurrentBossIndex = Random.Range(0, (int)EBossTypes.Count);

            //Testing
            CurrentBossIndex = 2;
           
            m_renderer.color = UniqueData.bosses[CurrentBossIndex].color;
            m_animator.runtimeAnimatorController = UniqueData.bosses[CurrentBossIndex].animControllerObject.GetComponent<Animator>().runtimeAnimatorController;
        }

        public override void OnDamageTaken(float damage)
        {
            base.OnDamageTaken(damage);

            if (m_activeHealth <= 0) 
            {
                m_hitbox.CanHit = false;
                m_hitbox.CanReceiveHit = false;
                Animator.SetTrigger("Die");
                Agent.isStopped = true;
                m_dyingAnimTimer = m_dyingAnimDelay;                                    
            }
        }

        private void DoBossDyingAnimAndUnspawn()
        {
            m_dyingAnimTimer -= Time.deltaTime;
            HandleSpriteFlashTimer();            

            if (m_dyingAnimTimer <= 0)
            {              
                m_hitbox.CanHit = true;
                m_hitbox.CanReceiveHit = true;
                m_dyingAnimTimer = 0.0f;
                m_parentPool.UnSpawn(gameObject);
                Animator.SetTrigger("ReturnToDefault");
            }
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
