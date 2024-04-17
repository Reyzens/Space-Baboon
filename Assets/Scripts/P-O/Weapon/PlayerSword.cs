using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class PlayerSword : PlayerWeapon
    {
        private Vector3 m_previousPosition;
        private Player m_playerDirection;
        [SerializeField] private Transform m_directionalHelper;

        protected override void Awake()
        {
            base.Awake();
            m_previousPosition = transform.position;
            m_playerDirection = transform.parent.GetComponent<Player>();
        }
        protected override void Attack()
        {
            Transform targetTransform = GetTarget();
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            var projectile = m_pool.Spawn(m_weaponData.projectilePrefab, spawnPos);

            projectile.GetComponent<Projectile>()?.Shoot(targetTransform, m_weaponData.maxRange, m_weaponData.attackZone, currentDamage, transform);
        }
        protected override void Update()
        {
            base.Update();
            if (m_playerDirection.GetPlayerDirection().magnitude > 0.0f)
            {
                m_directionalHelper.transform.position = transform.position + (Vector3)m_playerDirection.GetPlayerDirection();
            }
        }
        protected override Transform GetTarget()
        {
            return m_directionalHelper;
        }
    }
}
