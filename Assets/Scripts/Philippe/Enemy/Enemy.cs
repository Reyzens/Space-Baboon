using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : MonoBehaviour, IPoolable, IDamageable
    {
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private GameObject m_damageDoneObject;
        [SerializeField] private bool m_defending = false;
        
        private ObjectPool m_parentPool;
        private bool m_isActive = false;

        private Renderer m_renderer;
        private BoxCollider2D m_collider;
        private Rigidbody2D m_rb;
        
        private GameObject[] m_players;

        private GameObject m_prefab;
        private float m_health;
        private float m_damage;
        private float m_acceleration;
        private float m_maxVelocity;                    
        private float m_attackCooldown;
        private float m_attackTimer = 0.0f;
        private bool m_coolingDown = false;                

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
            m_acceleration = m_enemyData.baseAcceleration;
            m_maxVelocity = m_enemyData.baseVelocity;
            m_attackCooldown = m_enemyData.baseAttackCooldown;
            m_players = GameObject.FindGameObjectsWithTag("Player");
        }
        
        private void Update()
        {
            if (!m_isActive)
                return;            

            if (m_attackTimer <= 0.0f)
            {
                m_coolingDown = false;
            }
            if (m_coolingDown) 
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
            if (m_defending) // For testing purposes, à changer pour réagir aux projectiles à la place,
                             // ne pas avoir à choisir entre attacking et defending
            {
                if (collision.gameObject.tag == "Player" && m_attackTimer <= 0.0f)
                {
                    float damageValueTesting = 0.0f;
                    OnDamageTaken(damageValueTesting);
                }

                return;
            }

            if (collision.gameObject.tag == "Player" && m_attackTimer <= 0.0f) 
            {
                m_attackTimer = m_attackCooldown;
                m_coolingDown = true;
                Instantiate(m_damageDoneObject, transform.position, Quaternion.identity);
            }
        }

        private void MoveTowardsPlayer()
        {            
            Vector3 playerPosition = m_players[0].transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_rb.AddForce(direction * m_acceleration, ForceMode2D.Force);

            if (direction.magnitude > 0)
            {                
                RegulateVelocity();
            }
        }

        private void RegulateVelocity()
        {
            if (m_rb.velocity.magnitude > m_maxVelocity)
            {
                m_rb.velocity = m_rb.velocity.normalized;
                m_rb.velocity *= m_maxVelocity;
            }
        }

        public void OnDamageTaken(float values)
        {
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
        
    }
}
