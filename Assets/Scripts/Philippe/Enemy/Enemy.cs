using SpaceBaboon.PoolingSystem;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : Character, IPoolableGeneric
    {
        [SerializeField] private EnemyData m_enemyData;
        
        private GenericObjectPool m_parentPool;
        private bool m_isActive = false;

        private GameObject m_playerObject;
        private Player m_player;

        private float m_health;
        private float m_contactAttackTimer = 0.0f;
        private bool m_contactAttackReady = true;
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration; 
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;        

        private Vector2 m_vectorZero = Vector2.zero;

        private void Awake()
        {
            m_characterRenderer = GetComponent<Renderer>();
            m_characterCollider = GetComponent<BoxCollider2D>(); // TODO Change to circle collider for optimization
            m_characterRb = GetComponent<Rigidbody2D>();
            m_health = m_enemyData.defaultHeatlh;
        }

        private void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager
            m_player = m_playerObject.GetComponent<Player>();            
        }

        private void Update()
        {
            if (!m_isActive)
                return;

            if (!m_contactAttackReady)
                ReadyAttack();
        }

        private void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move(m_vectorZero);            
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
                Attack();                
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            SlightPushFromObstructingObject(collision);
        }

        private void SlightPushFromObstructingObject(Collision2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            m_characterRb.AddForce(-direction * m_enemyData.obstructionPushForce, ForceMode2D.Force);
        }      

        protected override void Move(Vector2 values)
        {
            MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            Vector3 playerPosition = m_playerObject.transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_characterRb.AddForce(direction * m_enemyData.defaultAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (direction.magnitude > 0)
                RegulateVelocity();
        }

        protected override void RegulateVelocity()
        {
            if (m_characterRb.velocity.magnitude > m_enemyData.defaultMaxVelocity /* + or * bonus */)
            {
                m_characterRb.velocity = m_characterRb.velocity.normalized;
                m_characterRb.velocity *= m_enemyData.defaultMaxVelocity /* + or * bonus */;
            }
        }

        private void ReadyAttack()
        {
            m_contactAttackTimer -= Time.deltaTime;

            if (m_contactAttackTimer < 0.0f)
                m_contactAttackReady = true;
        }

        private void Attack()
        {
            m_player.OnDamageTaken(m_enemyData.defaultContactAttackDamage);

            m_contactAttackTimer = m_enemyData.defaultContactAttackDelay /* + or * bonus */;
            m_contactAttackReady = false;
        }

        public override void OnDamageTaken(float values)
        {
            m_health -= values;

            Debug.Log("enemy hit have " + m_health + " health");
            if (m_health <= 0)
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
            m_characterRenderer.enabled = value;
            m_characterCollider.enabled = value;
        }
        #endregion
    }
}
