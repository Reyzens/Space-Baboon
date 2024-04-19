using SpaceBaboon.EnemySystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class EnemyWeapon : Weapon
    {
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private WeaponData m_weaponData;

        private GameObject m_enemySpawner;
        private EnemySpawner m_enemySpawnerScript;

        private Transform m_target;

        private void Start()
        {
            // TODO to change when we have gameManager
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

            var projectile = m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_projectilePrefab, spawnPos);

            projectile.GetComponent<Projectile>()?.Shoot(m_target, 0, 0, m_weaponData.baseDamage);
        }
    }
}
