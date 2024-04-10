using SpaceBaboon.EnemySystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class EnemyWeapon : Weapon
    {
        private GameObject m_enemySpawner;
        private EnemySpawner m_enemySpawnerScript;

        private Transform m_target;

        private void Start()
        {
            m_enemySpawner = GameObject.Find("EnemySpawner");
            m_enemySpawnerScript = m_enemySpawner.GetComponent<EnemySpawner>();


        }

        public void GetTarget(Transform target)
        {
            m_target = target;
            Attack();
        }

        protected override void Attack()
        {
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);

            var projectile = m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_enemySpawnerScript.m_shootingEnemyProjectile, spawnPos);

            projectile.GetComponent<Projectile>()?.Shoot(ref m_target, 0);
        }
    }
}
