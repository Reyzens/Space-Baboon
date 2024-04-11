using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class FlameThrower : PlayerWeapon
    {
        protected override void Attack()
        {
            Transform targetTransform = GetTarget();
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            var projectile = m_pool.Spawn(m_weaponData.projectilePrefab, spawnPos);

            projectile.GetComponent<Projectile>()?.Shoot(targetTransform, m_weaponData.maxRange, m_weaponData.attackZone, transform);
        }
    }
}
