using UnityEngine;
using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;

namespace SpaceBaboon.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GenericObjectPool m_enemyPool = new GenericObjectPool();
        [SerializeField] private List<GameObject> m_enemies = new List<GameObject>();

        [SerializeField] private GameObject m_map;
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private bool m_isSpawning = true;

        private Camera m_cam;
        private float m_spawningTimer = 0.0f;

        private void Awake()
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject enemyPrefab in m_enemies)
                list.Add(enemyPrefab);

            m_enemyPool.CreatePool(list, "test");
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
                    m_enemyPool.Spawn(m_enemies[0], spawnWorldPos);
                    //m_enemyPool.Spawn(m_enemies[1], spawnWorldPos);
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
    }
}
