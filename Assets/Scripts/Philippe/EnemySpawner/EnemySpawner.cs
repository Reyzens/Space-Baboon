using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public enum EEnemyTypes
    {
        Melee,
        Shooting,
        Kamikaze,
        Count
    }

    [System.Serializable]
    public struct EnemyToPool
    {
        public GameObject enemyPrefab;
        public EEnemyTypes enemyType;
        [Range(0, 10)] public int spawnProbability;
        public bool canSpawn;

        public void SetCanSpawn(bool value)
        {
            canSpawn = value;
        }
    }

    public class EnemySpawner : MonoBehaviour
    {
        // TODO Refactor: instead of GameObject list, create a list of struct
        [field: Header("OBJECT POOLS")]
        [SerializeField] private GenericObjectPool m_enemyPool = new GenericObjectPool();
        [SerializeField] private List<EnemyToPool> m_pooledEnemies = new List<EnemyToPool>();
        
        [SerializeField] public GenericObjectPool m_enemyProjectilesPool = new GenericObjectPool();
        [SerializeField] public GameObject m_shootingEnemyProjectile;
        [SerializeField] public GameObject m_explodingEnemyProjectile;

        [field: Header("SPAWNER LOGIC")]
        [SerializeField] private GameObject m_map; // TODO Change so we have a centralized map data, resource and enemy spawner could benefit from it
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private bool m_isSpawning = true;
        [SerializeField] private int m_enemiesAmountToSpawnOneShot = 0;
        // TODO maybe remove [SerializeField] of bool
        [SerializeField] private bool m_spawnGroup = false;

        private Camera m_cam;
        private float m_spawningTimer = 0.0f;

        private void Awake()
        {
            // TODO maybe check for a way to use same method, different parameter
            CreateEnemiesPool();   
            CreateEnemyProjectilesPool(); 
        }

        private void Start()
        {
            m_cam = Camera.main;
            m_spawningTimer = m_spawningDelay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) // Press C to add 1 enemy at will (for testing)
                CalculateSpawnPosition();

            if (m_isSpawning)
                SpawnWithTimer();

            if (m_spawnGroup)
                SpawnGroup(m_enemiesAmountToSpawnOneShot);
        }        

        private void SpawnWithTimer()
        {
            m_spawningTimer -= Time.deltaTime;

            if (m_spawningTimer <= 0.0f)
            {
                m_spawningTimer = m_spawningDelay;
                CalculateSpawnPosition();
            }
        }

        private void SpawnGroup(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                CalculateSpawnPosition();
            }
            m_spawnGroup = false;
        }

        private void CalculateSpawnPosition()
        {
            Vector2 mapMin = new Vector2(-(float)(m_map.transform.localScale.x * 0.5f), -(float)(m_map.transform.localScale.y * 0.5f));
            Vector2 mapMax = new Vector2((float)(m_map.transform.localScale.x * 0.5f), (float)(m_map.transform.localScale.y * 0.5f));
            Vector3 screenZeroWPos = m_cam.ScreenToWorldPoint(Vector3.zero);
            Vector3 screenCenterWPos = m_cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, m_cam.nearClipPlane));
            float screenCornerRadius = Vector3.Distance(screenZeroWPos, screenCenterWPos);

            float spawnRadius = screenCornerRadius + m_spawnRadiusFromScreenCorner;

            bool validPosFound = false;
            Vector3 spawnWorldPos = Vector3.zero;

            while (!validPosFound)
            {
                Vector2 randomPosOnCircle = RandomPosOnCircle(spawnRadius);
                Vector3 spawnPos = new Vector3(randomPosOnCircle.x, randomPosOnCircle.y, m_cam.nearClipPlane);

                spawnWorldPos = m_cam.transform.position + spawnPos;

                if (spawnWorldPos.x < mapMax.x &&
                   spawnWorldPos.y < mapMax.y &&
                   spawnWorldPos.x > mapMin.x &&
                   spawnWorldPos.y > mapMin.y)
                {
                    validPosFound = true;                    
                }
            }    

            ChooseRandomlyEnemyTypeToSpawn(spawnWorldPos);
        }

        private void ChooseRandomlyEnemyTypeToSpawn(Vector3 spawnWorldPos) // TODO maybe change method name so it better reflects what it does now with cheats integration
        {
            int totalProbabilities = 0;
            for (int i = 0; i < m_pooledEnemies.Count; i++)
            {
                if (m_pooledEnemies[i].canSpawn)
                {
                    totalProbabilities += m_pooledEnemies[i].spawnProbability;
                }
            }

            int randomValue = Random.Range(0, totalProbabilities);
            int cumulativeProbability = 0;

            for (int i = 0; i < m_pooledEnemies.Count; i++)
            {
                if (!m_pooledEnemies[i].canSpawn)
                {
                    continue;
                }

                cumulativeProbability += m_pooledEnemies[i].spawnProbability;
                
                if (randomValue < cumulativeProbability)
                {
                    if (i < m_pooledEnemies.Count && m_pooledEnemies[i].enemyPrefab != null)
                    {                        
                        m_enemyPool.Spawn(m_pooledEnemies[i].enemyPrefab, spawnWorldPos);
                    }
                    break;
                }
            }
        }

        private Vector2 RandomPosOnCircle(float radius)
        {
            float randomAngle = Random.Range(0.0f, Mathf.PI * 2.0f);

            float x = radius * Mathf.Cos(randomAngle);
            float y = radius * Mathf.Sin(randomAngle);

            return new Vector2(x, y);
        }

        private void CreateEnemiesPool()
        {
            List<GameObject> enemyList = new List<GameObject>();

            foreach (EnemyToPool enemy in m_pooledEnemies)
                enemyList.Add(enemy.enemyPrefab);

            m_enemyPool.CreatePool(enemyList, "Enemy");
        }

        private void CreateEnemyProjectilesPool()
        {
            List<GameObject> enemyProjectileList = new List<GameObject>();
            enemyProjectileList.Add(m_shootingEnemyProjectile);
            enemyProjectileList.Add(m_explodingEnemyProjectile);

            m_enemyProjectilesPool.CreatePool(enemyProjectileList, "Shooting Enemy Weapon Projectile");
        }

        public EnemySpawner GetEnemySpawner()
        {
            return this;
        }


        #region Cheats

        public void SetIsSpawning(bool value)
        {
            m_isSpawning = value;
        }

        public void SetDelay(float value)
        {
            m_spawningDelay = value;
        }

        public float GetDelay()
        {
            return m_spawningDelay;
        }

        public void ToggleSpawnByEnemyType(EEnemyTypes type, bool value)
        {
            m_pooledEnemies[(int)type].SetCanSpawn(value);
        }

        public void CheatSpawnGroup(EEnemyTypes type, int amount)
        {
            Debug.Log(m_pooledEnemies[(int)type].enemyPrefab.name + "   " + amount);

            
            //for (int i = 0; i < amount; i++)
            //{
            //    m_enemyPool.Spawn(m_pooledEnemies[(int)type].enemyPrefab, /* spawnWorldPos */);
            //}
        }
        #endregion
    }
}
