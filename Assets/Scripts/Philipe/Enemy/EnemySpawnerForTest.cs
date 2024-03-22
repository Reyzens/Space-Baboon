using UnityEngine;

namespace SpaceBaboon.Enemy
{
    public class EnemySpawnerForTest : MonoBehaviour
    {
        [SerializeField] private GameObject m_enemy;

        private Camera m_cam;        

        private void Start()
        {
            m_cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                Vector3 spawnPosition = Vector3.zero;
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                int randomScreenEdge = Random.Range(0, 4);                

                switch (randomScreenEdge) 
                {
                    case 0: // Top
                        spawnPosition = new Vector3(Random.Range(0.0f, screenWidth), screenHeight, m_cam.nearClipPlane);
                        break;
                    case 1: // Bottom
                        spawnPosition = new Vector3(Random.Range(0.0f, screenWidth), 0.0f, m_cam.nearClipPlane);
                        break;
                    case 2: // Left
                        spawnPosition = new Vector3(0.0f, Random.Range(0.0f, screenHeight), m_cam.nearClipPlane);
                        break;
                    case 3: // Right
                        spawnPosition = new Vector3(screenWidth, Random.Range(0.0f, screenHeight), m_cam.nearClipPlane);
                        break;
                    default:
                        break;
                
                }

                spawnPosition = m_cam.ScreenToWorldPoint(spawnPosition);
                Instantiate(m_enemy, spawnPosition, Quaternion.identity);
            }
        }
    }
}
