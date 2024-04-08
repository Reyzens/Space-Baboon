using SpaceBaboon.PoolingSystem;
using UnityEngine; 

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : Character, IPoolableGeneric
    {
        [SerializeField] protected EnemyData m_data;

        [SerializeField] private GameObject m_contactAttackParticleSystem; //TODO centralize to FX manager
        
        private GenericObjectPool m_parentPool;
        protected bool m_isActive = false;

        private GameObject m_playerObject;
        protected Player m_player;
                
        private float m_contactAttackTimer = 0.0f;
        protected bool m_contactAttackReady = true;
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration; 
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;        

        protected Vector2 m_noVectorValue = Vector2.zero;

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<BoxCollider2D>(); // TODO Change to circle collider for optimization
            m_rB = GetComponent<Rigidbody2D>();
            m_currentHealth = m_data.defaultHealth;
        }

        protected virtual void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = m_playerObject.GetComponent<Player>();            
        }

        protected virtual void Update()
        {
            if (!m_isActive)
                return;

            if (!m_contactAttackReady)
                ReadyContactAttack();
        }

        private void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move(m_noVectorValue);            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {                
                OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().OnHit());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && m_contactAttackReady)
            {
                ContactPoint2D contactPoint = collision.contacts[0];
                Vector2 contactPos = contactPoint.point;
                ContactAttack(contactPos);                
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            SlightPushFromObstructingObject(collision);
        }

        private void SlightPushFromObstructingObject(Collision2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            m_rB.AddForce(-direction * m_data.obstructionPushForce, ForceMode2D.Force);
        }      

        protected override void Move(Vector2 value)
        {
            MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            Vector3 playerPosition = m_playerObject.transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_rB.AddForce(direction * m_data.defaultAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (direction.magnitude > 0)
                RegulateVelocity();
        }

        protected override void RegulateVelocity()
        {
            if (m_rB.velocity.magnitude > m_data.defaultMaxVelocity /* + or * bonus */)
            {
                m_rB.velocity = m_rB.velocity.normalized;
                m_rB.velocity *= m_data.defaultMaxVelocity /* + or * bonus */;
            }
        }

        private void ReadyContactAttack()
        {
            m_contactAttackTimer -= Time.deltaTime;

            if (m_contactAttackTimer < 0.0f)
                m_contactAttackReady = true;
        }

        private void ContactAttack(Vector2 contactPos)
        {
            m_player.OnDamageTaken(m_data.defaultContactAttackDamage);

            InstantiateContactAttackParticuleSystem(contactPos);

            m_contactAttackTimer = m_data.defaultContactAttackDelay /* + or * bonus */;
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
            m_currentHealth -= damage;

            Debug.Log("enemy hit have " + m_currentHealth + " health");
            if (m_currentHealth <= 0)
            {
                m_parentPool.UnSpawn(gameObject);
            }
        } 

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
