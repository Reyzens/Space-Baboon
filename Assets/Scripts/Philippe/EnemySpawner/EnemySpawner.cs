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

    public class EnemySpawner : MonoBehaviour
    {
        [field: Header("OBJECT POOLS")]
        [SerializeField] private GenericObjectPool m_enemyPool = new GenericObjectPool();
        [SerializeField] private List<GameObject> m_enemyTypesToSpawn = new List<GameObject>();
        [SerializeField][Range(0, 10)] private List<int> m_spawnProbability = new List<int>();
        private List<bool> m_canSpawn = new List<bool>();

        [SerializeField] public GenericObjectPool m_enemyProjectilesPool = new GenericObjectPool();
        [SerializeField] public GameObject m_shootingEnemyProjectile;
        [SerializeField] public GameObject m_explodingEnemyProjectile;

        [field: Header("SPAWNER LOGIC")]
        [SerializeField] private GameObject m_map; // TODO Change so we have a centralized map data, resource and enemy spawner could benefit from it
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private bool m_isSpawning = true;

        private Camera m_cam;
        private float m_spawningTimer = 0.0f;

        private void Awake()
        {
            List<GameObject> enemyList = new List<GameObject>();

            foreach (GameObject enemyPrefab in m_enemyTypesToSpawn)
                enemyList.Add(enemyPrefab);

            m_enemyPool.CreatePool(enemyList, "Enemy");





            List<GameObject> enemyProjectileList = new List<GameObject>();
            enemyProjectileList.Add(m_shootingEnemyProjectile);
            enemyProjectileList.Add(m_explodingEnemyProjectile);

            m_enemyProjectilesPool.CreatePool(enemyProjectileList, "Shooting Enemy Weapon Projectile");


            foreach(GameObject enemyPrefab in m_enemyTypesToSpawn)
            {
                m_canSpawn.Add(true);
            }

            if (m_spawnProbability.Count != m_enemyTypesToSpawn.Count)
            {
                Debug.LogError("Spawn probability count is not the same as Enemy types to spawn count");
            }
            if (m_canSpawn.Count != m_enemyTypesToSpawn.Count)
            {
                Debug.LogError("Can spawn count is not the same as Enemy types to spawn count");
            }


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

            //if ()
            //{
            //
            //}
            // Toggle each enemy type spawning
            // 1 type with quantity

            ChooseRandomlyAnEnemyTypeToSpawn(spawnWorldPos);
        }

        private void ChooseRandomlyAnEnemyTypeToSpawn(Vector3 spawnWorldPos)
        {


            int totalProbabilities = 0;
            foreach (int probability in m_spawnProbability)
            {
                //if (m_canSpawn) 
                //{
                //
                //}
                totalProbabilities += probability;
            }

            int randomValue = Random.Range(0, totalProbabilities);
            int cumulativeProbability = 0;

            for (int i = 0; i < m_spawnProbability.Count; i++)
            {
                cumulativeProbability += m_spawnProbability[i];
                
                if (randomValue < cumulativeProbability)
                {
                    if (i < m_enemyTypesToSpawn.Count && m_enemyTypesToSpawn[i] != null)
                    {
                        GameObject enemyPrefab = m_enemyTypesToSpawn[i];
                        m_enemyPool.Spawn(enemyPrefab, spawnWorldPos);
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

        public void CheatSpawn(EEnemyTypes type, int amount)
        {
            // Exemple d'implémentation possible
            // À réévaluer lorsque tu auras une logique pour spawner différents ennemis

            // P.S. J'ai besoin d'un enum pour le type d'ennemi
            //      alors peut-être qu'on peut s'en servir pour l'index
            //      de m_enemyTypesToSpawn[EEnemyTypes.blabla] ?

       //    return;

       //    switch (type)
       //    {
       //        case EEnemyTypes.Melee:
       //            Vector2 somePosition = Vector2.zero;
       //            m_enemyPool.Spawn(m_enemyTypesToSpawn[0], somePosition);
       //            break;
       //        case EEnemyTypes.Shooting:
       //            break;
       //        case EEnemyTypes.Kamikaze:
       //            break;
       //        case EEnemyTypes.Count:
       //            break;
       //        default:
       //            break;
       //    }
       }
       #endregion
    }
}
