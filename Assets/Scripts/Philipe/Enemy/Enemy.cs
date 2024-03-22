using UnityEngine;

namespace SpaceBaboon.Enemy
{
    public class Enemy : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyData m_enemyData;
        [SerializeField] private GameObject m_damageDoneObject;
        
        private GameObject[] m_players;
        
        private float m_movementSpeed;
        private float m_damage;
        private float m_attackCooldown;
        private float m_attackTimer = 0.0f;
        private bool m_coolingDown = false;
        
        private void Start()
        {
            m_movementSpeed = m_enemyData.baseMovementSpeed;
            m_attackCooldown = m_enemyData.baseAttackCooldown;
            m_players = GameObject.FindGameObjectsWithTag("Player");
        }
        
        private void Update()
        {            
            MoveTowardsPlayer();

            if (m_attackTimer <= 0.0f)
            {
                m_coolingDown = false;
            }
            if (m_coolingDown) 
            {
                m_attackTimer -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && m_attackTimer <= 0.0f) 
            {
                m_attackTimer = m_attackCooldown;
                m_coolingDown = true;
                Instantiate(m_damageDoneObject, transform.position, Quaternion.identity);
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 playerPosition = Vector3.zero;
            playerPosition = m_players[0].transform.position;

            var step = m_movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);
        }



        public bool IsActive => throw new System.NotImplementedException();

        public void Activate(Vector2 pos, ObjectPool pool)
        {
            throw new System.NotImplementedException();
        }

        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }
        
        
    }
}
