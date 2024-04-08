using SpaceBaboon.EnemySystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ShootingEnemyWeapon : Weapon
    {
        private GameObject m_enemySpawner;
        private EnemySpawner m_enemySpawnerScript;

        private Transform m_target;

        protected override void Awake()
        {
            // Overriden so new pools are not created, since enemies will use a projectile pool in the enemy spawner
        }

        private void Start()
        {
            m_enemySpawner = GameObject.Find("EnemySpawner");
            m_enemySpawnerScript = m_enemySpawner.GetComponent<EnemySpawner>();            
        }

        public void GetTarget(Transform target)
        {            
            m_target = target;
            ShootGun();
            //Attack(); // TODO needs debugging
        }

        private void ShootGun()
        {
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            
            var projectile = m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_enemySpawnerScript.m_shootingEnemyProjectile, spawnPos);

            projectile.GetComponent<Projectile>()?.Shoot(ref m_target);
        }

        protected override void Attack()
        {
            // TODO Really bugged, there is probably a problem with heritage
            // Maybe the simplest thing would be to simply not have a weapon on shooting enemies

            //Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log("ENTERING ATTACK!!!!!!!!!!!!!!!");
            //
            //var projectile = m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_enemySpawnerScript.m_shootingEnemyProjectile, spawnPos);
            //
            //projectile.GetComponent<Projectile>()?.Shoot(ref m_target);
        }
    }
}
