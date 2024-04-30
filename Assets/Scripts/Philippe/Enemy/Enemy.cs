using SpaceBaboon.PoolingSystem;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : Character, IPoolableGeneric, IStatsEditable
    {
        public event Action m_eventEnemyDeath = delegate { };

        private EnemyData m_enemyUniqueData;

        [SerializeField] private GameObject m_contactAttackParticleSystem; //TODO centralize to FX manager

        protected GenericObjectPool m_parentPool;
        protected bool m_isActive = false;

        protected CircleCollider2D m_circleCollider;

        //private EnemySpawner m_enemySpawner;

        private GameObject m_playerObject;
        protected Player m_player;
        //private Color m_spriteRendererColor;
        //private float m_enemyFlashingTimer;

        protected float m_distanceToPlayer = 0.0f;
        private float m_contactAttackTimer = 0.0f;
        protected bool m_contactAttackReady = true;
        //private float m_bonusDamage = 0.0f;
        //private float m_bonusAcceleration;
        //private float m_bonusMaxVelocity;
        //private float m_bonusAttackDelay;

        protected Vector2 m_noVectorValue = Vector2.zero;

        protected NavMeshAgent m_navMeshAgent;

        protected override void Awake()
        {
            base.Awake();

            m_circleCollider = GetComponent<CircleCollider2D>();
            //m_spriteRendererColor = m_renderer.material.color;

            m_navMeshAgent = GetComponent<NavMeshAgent>();
            m_navMeshAgent.updateRotation = false;
            m_navMeshAgent.updateUpAxis = false;
        }

        protected virtual void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            m_enemyUniqueData = m_characterData as EnemyData;
            m_activeHealth = m_enemyUniqueData.defaultHealth;
        }

        protected override void Update()
        {
            if (!m_isActive)
                return;

            base.Update();

            CalculateDistanceToPlayer();

            if (!m_contactAttackReady)
                ReadyContactAttack();

            //if (m_enemyFlashingTimer > 0.0f)  //Refactored to Character
            //{
            //    m_enemyFlashingTimer -= Time.deltaTime;
            //
            //}
            //if (m_enemyFlashingTimer < 0.0f)
            //{
            //    m_renderer.material.color = m_spriteRendererColor;
            //}
        }

        protected virtual void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move(m_player.transform.position);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {            
        }

        protected void StopMovement()
        {
            m_navMeshAgent.acceleration = -m_characterData.defaultAcceleration;            
        }

        private void CalculateDistanceToPlayer()
        {
            m_distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
        }        

        protected override void Move(Vector2 value)
        {
            m_navMeshAgent.SetDestination(value);
            CheckForSpriteDirectionSwap(m_navMeshAgent.velocity);
        }        

        private void ReadyContactAttack()
        {
            m_contactAttackTimer -= Time.deltaTime;

            if (m_contactAttackTimer < 0.0f)
            {
                m_contactAttackReady = true;
            }
        }

        public void ContactAttack(Vector2 contactPos)
        {
            m_player.OnDamageTaken(m_enemyUniqueData.defaultContactAttackDamage);

            SpawnContactAttackVFX(contactPos);

            m_contactAttackTimer = m_enemyUniqueData.defaultContactAttackDelay /* + or * bonus */;
            m_contactAttackReady = false;
        }

        // TODO instantiation to be removed when particle system object pool integrated to project
        // TODO make sure particle system is at foreground
        protected void SpawnContactAttackVFX(Vector2 contactPos)
        {
            Vector3 contactPosVec = new Vector3(contactPos.x, contactPos.y, 2);

            Vector2 direction = m_playerObject.transform.position - contactPosVec;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            FXSystem.FXManager.Instance.PlayVFX(FXSystem.EVFXType.EnemySlashAttack, contactPosVec, rotation);

            //GameObject contactAttackInstance = Instantiate(m_contactAttackParticleSystem, contactPosVec, rotation);
            //
            //AudioSource contactAttackAS = contactAttackInstance.GetComponent<AudioSource>();
            //AudioClip contactAttackAC = contactAttackAS.clip;
            //contactAttackAS?.PlayOneShot(contactAttackAC);
        }

        // TODO this can be generalized to the parent most likely
        public override void OnDamageTaken(float damage)
        {
            m_activeHealth -= damage;
            //SpriteFlashing();
            SpriteFlashRed(m_spriteFlashTimer);
            DamagePopUp.Create(this.transform.position, damage);
            //Debug.Log(gameObject.name + " enemy hit -- now has " + m_activeHealth + " health");
            if (m_activeHealth <= 0)
            {
                m_eventEnemyDeath?.Invoke();
                m_parentPool.UnSpawn(gameObject);
            }
        }

        //// TODO this can be generalized to the parent :: Done
        //private void SpriteFlashing()
        //{
        //    m_enemyFlashingTimer = 0.2f;
        //    m_renderer.material.color = Color.red;
        //}
        public void registerPuzzle(CraftingPuzzle craftstation)
        {
            m_eventEnemyDeath += () => craftstation.PuzzleCounter();
        }
        public void UnregisterPuzzle(CraftingPuzzle craftstation)
        {
            m_eventEnemyDeath = null;
        }

        #region HitBox
        public virtual bool CanAttack()
        {
            return m_contactAttackReady;
        }
        #endregion

        #region Tools
        public override ScriptableObject GetData() { return m_characterData as EnemyData; }
        #endregion

        #region ObjectPooling
        public bool IsActive
        {
            get { return m_isActive; }
        }

        public void Activate(Vector2 pos, GenericObjectPool pool)
        {
            ResetValues(pos);
            SetComponents(true);
            m_parentPool = pool;
        }

        public void Deactivate()
        {
            SetComponents(false);
        }

        private void ResetValues(Vector2 pos)
        {
            transform.position = pos;
        }

        protected virtual void SetComponents(bool value)
        {
            m_isActive = value;
            m_renderer.enabled = value;
            m_circleCollider.enabled = value;
            m_navMeshAgent.isStopped = !value;
        }
        #endregion
    }
}
