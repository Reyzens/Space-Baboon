using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class EnemyProjectile : Enemy
    {
        [SerializeField] private GameObject m_projectile;        
        private float m_distanceToPlayer;
        
        protected override void Update()
        {
            base.Update();
            CalculateDistanceToPlayer();
            //Debug.Log("DistanceToPlayer" + m_distanceToPlayer);
            
            if (m_attackReady && m_distanceToPlayer < m_enemyData.baseAttackRange /* + or * bonus */)
            {
                Attack(); // TODO a rechecker aussi pcq ça aurait rapport avec la maniere dont l'attaque est faites               
            }
        }        

        protected override void Move()
        {
            if (m_distanceToPlayer < m_enemyData.baseAttackRange /* + or * bonus */)
            {
                if (m_rb.velocity.magnitude < 0.1f)
                    m_rb.velocity = Vector2.zero;

                StopMovement();
                return;
            }

            base.Move(); // TODO à rechecker
        }

        protected override void Attack()
        {
            Vector3 playerPosition = m_players[0].transform.position;
            GameObject projectile = Instantiate(m_projectile, transform.position, Quaternion.identity);
            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();

            Vector2 direction = (playerPosition - transform.position).normalized;
            projectileRB.AddForce(direction * 10, ForceMode2D.Impulse);
            m_attackTimer = m_enemyData.baseAttackDelay;
            m_attackReady = false;
            
        }

        private void StopMovement()
        {
            m_rb.AddForce(-m_rb.velocity.normalized * 10, ForceMode2D.Force);
        }

        private void CalculateDistanceToPlayer()
        {
            m_distanceToPlayer = Vector3.Distance(transform.position, m_players[0].transform.position);
        }







    }
}
