using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        //[SerializeField] private GameObject m_meleeEnemy;
        //[SerializeField] private ObjectPool m_meleeEnemyPool;
        //[SerializeField] private GameObject m_projectileEnemy;
        //[SerializeField] private ObjectPool m_projectileEnemyPool;

        [SerializeField] private List<GameObject> m_enemyPrefabs;

        [SerializeField] private GameObject m_map;
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private bool m_isSpawning = true;

        private Dictionary<GameObject, ObjectPool> m_enemyDictionary = new Dictionary<GameObject, ObjectPool>();

        private Camera m_cam;
        private float m_spawningTimer = 0.0f;

        private void Awake()
        {
            //m_meleeEnemyPool.CreatePool(m_meleeEnemy);
            //m_projectileEnemyPool.CreatePool(m_projectileEnemy);

            foreach (GameObject resource in m_enemyPrefabs)
            {
                if (!m_enemyDictionary.ContainsKey(resource))
                {
                    m_enemyDictionary.Add(resource, new ObjectPool());
                }
            }
        }

        private void Start()
        {
            m_cam = Camera.main;
            m_spawningTimer = m_spawningDelay;

            foreach (KeyValuePair<GameObject, ObjectPool> enemy in m_enemyDictionary)
            {
                enemy.Value.SetPoolSize(10);
                enemy.Value.CreatePool(enemy.Key);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) // Press C to add 1 enemy at will (for testing)
            {
                int enemyIndex = Random.Range(0, m_enemyPrefabs.Count);
                Vector3 spawnWorldPos = CalculateSpawnPosition();

                m_enemyDictionary[m_enemyPrefabs[enemyIndex]].Spawn(spawnWorldPos);

            }
                

            if (m_isSpawning)
                SpawnWithTimer();
        }

        private void SpawnWithTimer()
        {
            m_spawningTimer -= Time.deltaTime;

            if (m_spawningTimer <= 0.0f)
            {
                m_spawningTimer = m_spawningDelay;
                Vector3 spawnWorldPos = CalculateSpawnPosition();

                int enemyIndex = Random.Range(0, m_enemyPrefabs.Count);

                if (m_enemyDictionary[m_enemyPrefabs[enemyIndex]].GetPoolQueue() != 0)
                {
                    m_enemyDictionary[m_enemyPrefabs[enemyIndex]].Spawn(spawnWorldPos);
                    //canSpawn = false;
                }
                else
                {
                    //poolAmountTested += m_poolSize;
                    //canSpawn = true;
                }

                //m_projectileEnemyPool.Spawn(spawnWorldPos);
            }
        }

        private Vector3 CalculateSpawnPosition()
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

                if(spawnWorldPos.x < mapMax.x &&
                   spawnWorldPos.y < mapMax.y &&
                   spawnWorldPos.x > mapMin.x &&
                   spawnWorldPos.y > mapMin.y)
                {
                    validPosFound = true;
                    return spawnWorldPos;                    
                }
            }

            return Vector3.zero;
        }

        private Vector2 RandomPosOnCircle(float radius)
        {
            float randomAngle = Random.Range(0.0f, Mathf.PI * 2.0f);

            float x = radius * Mathf.Cos(randomAngle);
            float y = radius * Mathf.Sin(randomAngle);

            return new Vector2(x, y);
        }
    }
}
