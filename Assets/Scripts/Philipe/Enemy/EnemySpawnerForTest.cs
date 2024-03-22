using UnityEngine;

namespace SpaceBaboon.Enemy
{
    public class EnemySpawnerForTest : MonoBehaviour
    {
        [SerializeField] private GameObject m_enemy;

        [SerializeField] private ObjectPool m_pool;
        
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;

        private Camera m_cam;

        //private void Awake()
        //{
        //    m_pool.CreatePool(m_enemy);
        //}

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
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            Vector3 screenZeroWPosition = m_cam.ScreenToWorldPoint(Vector3.zero);
            Vector3 screenCenterWPosition = m_cam.ScreenToWorldPoint(new Vector3(screenWidth * 0.5f, screenHeight * 0.5f, m_cam.nearClipPlane));
            float screenCornerRadius = Vector3.Distance(screenZeroWPosition, screenCenterWPosition);

            float spawnRadius = screenCornerRadius + m_spawnRadiusFromScreenCorner;

            Vector2 randomPosOnCircle = RandomPosOnCircle(spawnRadius);
            Vector3 spawnPosition = new Vector3(randomPosOnCircle.x, randomPosOnCircle.y, m_cam.nearClipPlane);

            Vector3 worldSpawnPosition = m_cam.transform.position + spawnPosition;

            Instantiate(m_enemy, worldSpawnPosition, Quaternion.identity);
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






/*
 * 
 * 
 * 
 if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 spawnPosition = Vector3.zero;
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;                               

                Vector3 zeroPos = Vector3.zero;
                //Vector3 screenZeroWPosition = m_cam.ScreenToWorldPoint(zeroPos);
                Vector3 screenZeroWPosition = Vector3.zero;
                Vector3 screenCenterWPosition = m_cam.ScreenToWorldPoint(new Vector3(screenWidth * 0.5f, screenHeight * 0.5f, m_cam.nearClipPlane));
                //Vector3 screenCenterWPosition = new Vector3(screenWidth * 0.5f, screenHeight * 0.5f, m_cam.nearClipPlane);
                Debug.Log(screenCenterWPosition);
                float screenCornerRadius = Vector3.Distance(screenZeroWPosition, screenCenterWPosition);

                float spawnRadius = screenCornerRadius + m_spawnRadiusFromScreenCorner;

                //Debug.Log(spawnRadius);



                Vector2 randomPosOnCircle = RandomPosOnCircle(spawnRadius);

                spawnPosition = new Vector3(randomPosOnCircle.x, randomPosOnCircle.y, m_cam.nearClipPlane);

                Vector3 worldSpawnPos = new Vector3(m_cam.transform.position.x + spawnPosition.x, m_cam.transform.position.y + spawnPosition.y, m_cam.nearClipPlane);



                Instantiate(m_enemy, worldSpawnPos, Quaternion.identity);

                // Spawn, mettre object pool ici
            }




 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 //int randomScreenEdge = Random.Range(0, 4);                
                //
                //switch (randomScreenEdge) 
                //{
                //    case 0: // Top
                //        spawnPosition = new Vector3(Random.Range(0.0f, screenWidth), screenHeight, m_cam.nearClipPlane);
                //        break;
                //    case 1: // Bottom
                //        spawnPosition = new Vector3(Random.Range(0.0f, screenWidth), 0.0f, m_cam.nearClipPlane);
                //        break;
                //    case 2: // Left
                //        spawnPosition = new Vector3(0.0f, Random.Range(0.0f, screenHeight), m_cam.nearClipPlane);
                //        break;
                //    case 3: // Right
                //        spawnPosition = new Vector3(screenWidth, Random.Range(0.0f, screenHeight), m_cam.nearClipPlane);
                //        break;
                //    default:
                //        break;
                //
                //}
                //
                //spawnPosition = m_cam.ScreenToWorldPoint(spawnPosition);
                //Instantiate(m_enemy, spawnPosition, Quaternion.identity);


                //float halfScreenWidth = Screen.width / 2;
                //float halfScreenHeight = Screen.height / 2;
                //float screenCornerRadius = halfScreenWidth * halfScreenHeight;
 
 private void RespawnPlayerRandomCircle(GameObject player)
    {
        GameAudioManager.Instance.PlayKnockedOutSFX();
        Vector2 randomPosOnCircle = RandomPosOnCircle();
        Vector3 randomPosition = new Vector3(randomPosOnCircle.x, m_respawnHeight, randomPosOnCircle.y);

        player.transform.position = randomPosition;
        player.transform.LookAt(new Vector3(0, m_respawnHeight, 0));
    }

    private Vector2 RandomPosOnCircle()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        float x = m_radius * Mathf.Cos(randomAngle);
        float y = m_radius * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }
 
 
 
 
 
 
 
 
 
 
 */