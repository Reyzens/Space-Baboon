using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : MonoBehaviour, IPoolable, IDamageable
    {
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private GameObject m_damageDoneObject;        
        [SerializeField] private float m_health;

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
        private float m_bonusAttackCooldown;
        private float m_attackTimer = 0.0f;
        private bool m_attackCoolingDown = false;                

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

            if (m_attackTimer <= 0.0f)
            {
                m_attackCoolingDown = false;
            }
            if (m_attackCoolingDown) 
            {
                m_attackTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (!m_isActive)
                return;

            MoveTowardsPlayer();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && m_attackTimer <= 0.0f) 
            {
                m_attackTimer = m_enemyData.baseAttackCooldown /* + or * bonus */;
                m_attackCoolingDown = true;
                Instantiate(m_damageDoneObject, transform.position, Quaternion.identity);
            }

            if (collision.gameObject.CompareTag("Projectile"))
            {                
                OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().GetDamage());
            }
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
        
        public float GetDamage()
        {
            return m_enemyData.baseDamage + m_bonusDamage /* + or * bonus */;
        }
    }
}
