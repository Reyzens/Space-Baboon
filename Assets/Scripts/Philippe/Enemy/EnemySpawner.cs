using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_meleeEnemy;
        [SerializeField] private ObjectPool m_meleeEnemyPool;        
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private GameObject m_map;

        private Camera m_cam;

        private void Awake()
        {
            m_meleeEnemyPool.CreatePool(m_meleeEnemy);            
        }

        private void Start()
        {
            m_cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
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

            bool spawnPosFound = false;
            Vector3 spawnWorldPos = Vector3.zero;

            while (!spawnPosFound) 
            {
                Vector2 randomPosOnCircle = RandomPosOnCircle(spawnRadius);
                Vector3 spawnPos = new Vector3(randomPosOnCircle.x, randomPosOnCircle.y, m_cam.nearClipPlane);

                spawnWorldPos = m_cam.transform.position + spawnPos;

                if(spawnWorldPos.x < mapMax.x
                    && spawnWorldPos.y < mapMax.y
                    && spawnWorldPos.x > mapMin.x
                    && spawnWorldPos.y > mapMin.y)
                {
                    spawnPosFound = true;
                    m_meleeEnemyPool.Spawn(spawnWorldPos);
                }
            }           
        }

        private Vector2 RandomPosOnCircle(float radius)
        {
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);

            float x = radius * Mathf.Cos(randomAngle);
            float y = radius * Mathf.Sin(randomAngle);

            return new Vector2(x, y);
        }



    }
}
