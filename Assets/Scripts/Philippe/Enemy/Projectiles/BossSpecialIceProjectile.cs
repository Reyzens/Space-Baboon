using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class BossSpecialIceProjectile : Projectile
    {
        private Vector2 m_playerSavedPos = Vector2.zero;
        private float m_distanceToPosThreshold = 2.0f;
        private bool m_isAtTargetPos = false;

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            if (m_isAtTargetPos)
            {
                return;
            }

            Move();
        }

        protected override void Move()
        {
            m_direction.x = m_playerSavedPos.x - transform.position.x;
            m_direction.y = m_playerSavedPos.y - transform.position.y;
            m_direction = m_direction.normalized;

            float distanceToPos = Vector2.Distance(transform.position, m_playerSavedPos);

            Debug.Log("distance to pos " + distanceToPos);

            if (distanceToPos > m_distanceToPosThreshold)
            {
                m_rb.AddForce(m_direction * m_projectileData.defaultAcceleration, ForceMode2D.Force);
                RegulateVelocity();
            }
            else
            {
                //m_isAtTargetPos = true;
                //m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                m_rb.velocity = Vector2.zero;
            }          
        }        

        public override void Shoot(Transform direction, float maxRange, float attackZone, float damage, Transform playerPosition)
        {
            m_playerSavedPos = direction.position;
            Vector2 newDirection = direction.position;
            Vector2 currentPosition = transform.position;
            m_direction = (newDirection - currentPosition).normalized;
            m_damage = damage;
        }





    }
}
