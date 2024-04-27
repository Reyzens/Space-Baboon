using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ShootingEnemyProjectile : Projectile
    {
        private GameObject m_playerObject;
        protected Player m_player;

        protected void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = m_playerObject.GetComponent<Player>();
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            Move();
        }

        public override void Shoot(Transform direction, float maxRange, float attackZone, float damage, Transform playerPosition)
        {
            Vector2 newDirection = direction.position;
            Vector2 currentPosition = transform.position;
            m_direction = (newDirection - currentPosition).normalized;
            m_damage = damage;
        }

        public override float OnHit()
        {
            m_parentPool.UnSpawn(gameObject);
            return base.OnHit();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {           
            if (m_parentPool != null)
                m_parentPool.UnSpawn(gameObject);
        }
    }
}
