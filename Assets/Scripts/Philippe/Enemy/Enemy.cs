using UnityEngine;
using SpaceBaboon.PoolingSystem;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : MonoBehaviour, IPoolable, IDamageable
    {
        [SerializeField] private EnemyData m_enemyData;           
        [SerializeField] private float m_health;
        [SerializeField] private float m_obstructionPushForce = 5.0f;

        private ObjectPool m_parentPool;
        private bool m_isActive = false;

        private Renderer m_renderer;
        private BoxCollider2D m_collider;
        private Rigidbody2D m_rb;
        
        private GameObject[] m_players; // TODO change how to get reference to player

        private GameObject m_prefab;        
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration; // Pour simplifier on pourrait simplement avoir une acceleration de base qui ne change pas et un max Velocity qui peut changer
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;
        private float m_attackTimer = 0.0f;                     
        private bool m_attackReady = true;                   

        private void Awake()
        {
            m_renderer = GetComponent<Renderer>();
            m_collider = GetComponent<BoxCollider2D>();
            m_rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            m_prefab = m_enemyData.prefab;
            m_health = m_enemyData.baseHealth;                        
            m_players = GameObject.FindGameObjectsWithTag("Player");            
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

            MoveTowardsPlayer();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {            
            if (collision.gameObject.CompareTag("Projectile"))
            {                
                OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().GetDamage());
            }                       
        }

        private void OnCollisionStay2D(Collision2D collision)
        {                       
            SlightPushFromObstructingObject(collision);
        }

        private void SlightPushFromObstructingObject(Collision2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            m_rb.AddForce(-direction * m_enemyData.obstructionPushForce, ForceMode2D.Force);            
        }

        private void ReadyAttack()
        {
            m_attackTimer -= Time.deltaTime;

            if (m_attackTimer < 0.0f)
                m_attackReady = true;            
        }

        private void MoveTowardsPlayer()
        {            
            Vector3 playerPosition = m_players[0].transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_rb.AddForce(direction * m_enemyData.baseAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (direction.magnitude > 0)
                RegulateVelocity();
        }

        private void RegulateVelocity()
        {
            if (m_rb.velocity.magnitude > m_enemyData.baseMaxVelocity /* + or * bonus */)
            {
                m_rb.velocity = m_rb.velocity.normalized;
                m_rb.velocity *= m_enemyData.baseMaxVelocity /* + or * bonus */;
            }
        }

        public void OnDamageTaken(float values)
        {
            m_health -= values;

            if (m_health <= 0)
                m_parentPool.UnSpawn(gameObject);
        }

        public float GetDamage() // TODO a discuter, fonctionnement de cette methode
        {
            if(!m_attackReady)
                return 0.0f;

            Attack();
            return m_enemyData.baseDamage /* + or * bonus */;
        }

        private void Attack()
        {            
            m_attackTimer = m_enemyData.baseAttackDelay /* + or * bonus */;
            m_attackReady = false;
        }

        #region ObjectPool
        public bool IsActive
        {
            get { return m_isActive; }
            //set { m_isActive = value; } // private set ?
        }

        public void Activate(Vector2 pos, ObjectPool pool)
        {
            m_isActive = true;
            SetComponents(true);
            transform.position = pos;
            m_parentPool = pool;
        }

        public void Deactivate()
        {
            m_isActive = false;
            SetComponents(false);
        }

        private void SetComponents(bool value)
        {
            m_renderer.enabled = value;
            m_collider.enabled = value;
        }
        #endregion
    }
}
