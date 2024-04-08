using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ShootingEnemyProjectile : Projectile
    {
        // TODO projectiles needs to go through other enemies, no behavior when colliding with enemies

        private GameObject m_playerObject;
        protected Player m_player;

        protected override void Start()
        {
            base.Start();
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = m_playerObject.GetComponent<Player>();
        }

        public override void Shoot(ref Transform direction)
        {
            Vector2 newDirection = direction.position;
            Vector2 currentPosition = transform.position;
            m_direction = (newDirection - currentPosition).normalized;            
        }        

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                m_player.OnDamageTaken(m_projectileData.damage);
                m_parentPool.UnSpawn(gameObject);
            }
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                m_parentPool.UnSpawn(gameObject);
            }
        }
    }
}
