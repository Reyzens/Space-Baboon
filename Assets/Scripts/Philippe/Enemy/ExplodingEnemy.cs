using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class ExplodingEnemy : Enemy
    {
        private ExplodingEnemyData m_uniqueData;        

        private GameObject m_enemySpawner;
        private EnemySpawner m_enemySpawnerScript;



        protected override void Start()
        {
            base.Start();

            m_uniqueData = m_data as ExplodingEnemyData;
            
            // Maybe randomize distance to trigger bomb from data

            m_enemySpawner = GameObject.Find("EnemySpawner");
            m_enemySpawnerScript = m_enemySpawner.GetComponent<EnemySpawner>();
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            if (m_distanceToPlayer < m_uniqueData.minDistanceForTriggeringBomb)
            {
                m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_enemySpawnerScript.m_explodingEnemyProjectile, transform.position);
                m_parentPool.UnSpawn(gameObject);
            }           
        }
    }
}
