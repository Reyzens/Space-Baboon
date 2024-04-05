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
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration; 
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;
        private float m_attackTimer = 0.0f;
        private bool m_attackReady = true;

        private void Awake()
        {
            m_characterRenderer = GetComponent<Renderer>();
            m_characterCollider = GetComponent<BoxCollider2D>(); // TODO Change to circle collider for optimization
            m_characterRb = GetComponent<Rigidbody2D>();
            m_health = m_enemyData.DefaultBaseHeatlh;
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

            if (!m_attackReady)
                ReadyAttack();
        }

        private void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                //Debug.Log("Received " + collision.gameObject.GetComponent<WeaponSystem.Projectile>().OnHit() + " damage");
                OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().OnHit());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && m_attackReady)
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

        private void ReadyAttack()
        {
            m_attackTimer -= Time.deltaTime;

            if (m_attackTimer < 0.0f)
                m_attackReady = true;
        }

        protected override void Move()
        {
            MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            Vector3 playerPosition = m_playerObject.transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_characterRb.AddForce(direction * m_enemyData.DefaultBaseAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (direction.magnitude > 0)
                RegulateVelocity();
        }

        protected override void RegulateVelocity()
        {
            if (m_characterRb.velocity.magnitude > m_enemyData.DefaultBaseMaxVelocity /* + or * bonus */)
            {
                m_characterRb.velocity = m_characterRb.velocity.normalized;
                m_characterRb.velocity *= m_enemyData.DefaultBaseMaxVelocity /* + or * bonus */;
            }
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

        public float GetDamage() // TODO a discuter, fonctionnement de cette methode
        {
            if (!m_attackReady)
                return 0.0f;

            Attack();
            return m_enemyData.baseDamage /* + or * bonus */;
        }

        private void Attack()
        {  
            m_player.ReceiveDamage(m_enemyData.baseDamage);

            m_attackTimer = m_enemyData.baseAttackDelay /* + or * bonus */;
            m_attackReady = false;
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
