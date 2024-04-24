using SpaceBaboon.PoolingSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : Character, IPoolableGeneric, IStatsEditable
    {
        public event Action m_eventEnemyDeath = delegate { };
        
        private EnemyData m_enemyUniqueData;

        [SerializeField] private GameObject m_contactAttackParticleSystem; //TODO centralize to FX manager

        protected GenericObjectPool m_parentPool;
        protected bool m_isActive = false;

        //private EnemySpawner m_enemySpawner;

        private GameObject m_playerObject;
        protected Player m_player;

        protected float m_distanceToPlayer = 0.0f;
        private float m_contactAttackTimer = 0.0f;
        protected bool m_contactAttackReady = true;
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration;
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;

        protected Vector2 m_noVectorValue = Vector2.zero;

        protected virtual void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<BoxCollider2D>(); // TODO Change to circle collider for optimization
            m_rB = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            m_enemyUniqueData = m_characterData as EnemyData;
            m_activeHealth = m_enemyUniqueData.defaultHealth;
        }

        protected virtual void Update()
        {
            if (!m_isActive)
                return;

            CalculateDistanceToPlayer();

            if (!m_contactAttackReady)
                ReadyContactAttack();
        }

        protected virtual void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move(m_noVectorValue);
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Debug.Log("OnTriggerEnter" + gameObject.name);
        //    //if (collision.gameObject.CompareTag("Projectile"))
        //    //{
        //    //    OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().OnHit());
        //    //}
        //}

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Player") && m_contactAttackReady)
        //    {
        //        ContactPoint2D contactPoint = collision.contacts[0];
        //        Vector2 contactPos = contactPoint.point;
        //        ContactAttack(contactPos);
        //    }
        //}

        private void OnCollisionStay2D(Collision2D collision)
        {
            SlightPushFromObstructingObject(collision);
        }

        protected void StopMovement()
        {
            m_rB.AddForce(-m_rB.velocity.normalized * m_characterData.defaultAcceleration, ForceMode2D.Force);
        }

        private void CalculateDistanceToPlayer()
        {
            m_distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
        }        

        protected virtual void SlightPushFromObstructingObject(Collision2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            m_rB.AddForce(-direction * m_enemyUniqueData.obstructionPushForce, ForceMode2D.Force);
        }

        protected override void Move(Vector2 value)
        {
            MoveTowardsPlayer();
            CheckForSpriteDirectionSwap(m_movementDirection);
        }

        protected virtual void MoveTowardsPlayer()
        {
            Vector3 playerPosition = m_playerObject.transform.position;

            m_movementDirection = (playerPosition - transform.position).normalized;
            m_rB.AddForce(m_movementDirection * m_enemyUniqueData.defaultAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (m_movementDirection.magnitude > 0)
                RegulateVelocity();
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
        
            InstantiateContactAttackParticuleSystem(contactPos);
        
            m_contactAttackTimer = m_enemyUniqueData.defaultContactAttackDelay /* + or * bonus */;
            m_contactAttackReady = false;
        }

        // TODO intantiation to be removed when particle system object pool integrated to project
        // TODO make sure particle system is at foreground
        private void InstantiateContactAttackParticuleSystem(Vector2 contactPos)
        {
            Vector3 contactPosVec = new Vector3(contactPos.x, contactPos.y, 2);

            Vector2 direction = m_playerObject.transform.position - contactPosVec;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject contactAttackInstance = Instantiate(m_contactAttackParticleSystem, contactPosVec, rotation);

            AudioSource contactAttackAS = contactAttackInstance.GetComponent<AudioSource>();
            AudioClip contactAttackAC = contactAttackAS.clip;
            contactAttackAS?.PlayOneShot(contactAttackAC);
        }

        public override void OnDamageTaken(float damage)
        {
            m_activeHealth -= damage;

            Debug.Log(gameObject.name + " enemy hit -- now has " + m_activeHealth + " health");
            if (m_activeHealth <= 0)
            {
                m_eventEnemyDeath?.Invoke();
                m_parentPool.UnSpawn(gameObject);
            }
        }

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

        private void SetComponents(bool value)
        {
            m_isActive = value;
            m_renderer.enabled = value;
            m_collider.enabled = value;
        }
        #endregion
    }
}
