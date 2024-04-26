using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace SpaceBaboon.EnemySystem
{
    public enum EEnemyTypes
    {
        Melee,
        Shooting,
        Kamikaze,
        Boss,
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
        [field: Header("OBJECT POOLS")]
        [SerializeField] private GenericObjectPool m_enemyPool = new GenericObjectPool();
        [SerializeField] private List<EnemyToPool> m_pooledEnemies = new List<EnemyToPool>();        

        [SerializeField] public GenericObjectPool m_enemyProjectilesPool = new GenericObjectPool();        
        [SerializeField] public List<GameObject> m_pooledEnemyProjectiles = new List<GameObject>();

        [field: Header("SPAWNER LOGIC")]        
        [SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f; // TODO check this out, not sure why the radius is so huge that I need to add negative distance 
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private bool m_isSpawning = true;
        [SerializeField] private int m_enemiesAmountToSpawnOneShot = 0;
        // TODO maybe remove [SerializeField] of bool
        [SerializeField] private bool m_spawnGroup = false;

        private Camera m_cam;
        private float m_spawningTimer = 0.0f;

        [SerializeField] private Tilemap m_tilemapRef;
        [SerializeField] private Tilemap m_obstacleTileMapRef;
        private List<Vector3> m_spawnPositionsAvailable = new List<Vector3>();
        private float m_minDistanceFromAMapCollider = 50.0f;


        private void Awake()
        {
            CreateEnemySpawnerPools();
        }

        private void Start()
        {
            m_cam = Camera.main;
            m_spawningTimer = m_spawningDelay;
            GenerateGrid();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C)) // Press C to add 1 enemy at will (for testing)
                SpawnOneEnemy();

            if (m_isSpawning)
                SpawnWithTimer();

            if (m_spawnGroup)
                SpawnGroup(m_enemiesAmountToSpawnOneShot);
        }

        private void GenerateGrid()
        {
            foreach (var positions in m_tilemapRef.cellBounds.allPositionsWithin)
            {
                if (m_tilemapRef.HasTile(positions))
                {                    
                    m_spawnPositionsAvailable.Add(m_tilemapRef.CellToWorld(positions));
                }
            }
        }

        //private void GenerateGrid2()
        //{
        //    foreach (var positions in m_tilemapRef.cellBounds.allPositionsWithin)
        //    {
        //        if (m_tilemapRef.HasTile(positions))
        //        {
        //
        //            Vector3 currentWorldPos = m_tilemapRef.CellToWorld(positions);
        //
        //            if (IsFarEnoughFromObstacle(currentWorldPos))
        //            {
        //                m_spawnPositionsAvailable.Add(currentWorldPos);
        //            }
        //
        //            //m_spawnPositionsAvailable.Add(m_tilemapRef.CellToWorld(positions));
        //        }
        //    }
        //}
        //
        //private bool IsFarEnoughFromObstacle(Vector3 currentWorldPos)
        //{
        //    foreach (var obstaclePosition in m_obstacleTileMapRef.cellBounds.allPositionsWithin)
        //    {
        //        if (m_obstacleTileMapRef.HasTile(obstaclePosition))
        //        {                    
        //            Vector3 obstacleWorldPos = m_obstacleTileMapRef.CellToWorld(obstaclePosition);
        //                                
        //            float distance = Vector3.Distance(obstacleWorldPos, currentWorldPos);
        //                                
        //            if (distance < m_minDistanceFromAMapCollider)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    
        //    return true;
        //}



        private void SpawnOneEnemy()
        {
            //Debug.Log("Try to spawn one");
            Vector3 spawnWorldPos = FindValidEnemyRandomPos();
            m_enemyPool.Spawn(GetRandomEnemyType(), spawnWorldPos);
        }

        private void SpawnWithTimer()
        {
            m_spawningTimer -= Time.deltaTime;

            if (m_spawningTimer <= 0.0f)
            {
                m_spawningTimer = m_spawningDelay;
                SpawnOneEnemy();
            }
        }        

        private void SpawnGroup(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnOneEnemy();
            }
            m_spawnGroup = false;
        }

        private Vector3 FindValidEnemyRandomPos()
        {             
            float spawnRadius = CalculateValidSpawnRadius();
            return RandomValidPosOnCircleAroundPlayer(spawnRadius);
        }   

        private GameObject GetRandomEnemyType()
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
                        return m_pooledEnemies[i].enemyPrefab;
                    }
                    break;
                }
            }

            Debug.LogError("Did not find an enemy type");
            return null;
        }

        private Vector3 RandomValidPosOnCircleAroundPlayer(float radius)
        {
            Vector3 validTilePos = Vector3.zero;
            Vector3Int currentPlayerTilePos = m_tilemapRef.WorldToCell(m_cam.transform.position);
            int radiusThreshold = 1;

            List<Vector3Int> positionsNearRadius = new List<Vector3Int>();

            foreach (var tilePos in m_tilemapRef.cellBounds.allPositionsWithin)
            {
                if (m_tilemapRef.HasTile(tilePos))
                {
                    float distance = Vector3Int.Distance(currentPlayerTilePos, tilePos);
                    if (distance > radius - radiusThreshold && distance < radius + radiusThreshold)
                    {
                        positionsNearRadius.Add(tilePos);
                    }
                }
            }

            if (positionsNearRadius.Count > 0)
            {
                int randomIndex = Random.Range(0, positionsNearRadius.Count);
                validTilePos = m_tilemapRef.CellToWorld(positionsNearRadius[randomIndex]) + new Vector3(0.5f, 0.5f, 0f);
            }
            else 
            {
                Debug.Log("No valid pos found on circle");
            }

            return validTilePos;
        }

        private float CalculateValidSpawnRadius()
        {
            Vector3 screenZeroWPos = m_cam.ScreenToWorldPoint(Vector3.zero);
            Vector3 screenCenterWPos = m_cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, m_cam.nearClipPlane));
            float screenCornerRadius = Vector3.Distance(screenZeroWPos, screenCenterWPos);

            return screenCornerRadius + m_spawnRadiusFromScreenCorner;
        }

        private bool CheckPosValidity(Vector2 positionToTest)
        {
            //Check if the position is valid
            Collider2D colliderOnPos = Physics2D.OverlapPoint(positionToTest);

            if (colliderOnPos != null) { return false; }

            return true;
        }

        private void CreateEnemySpawnerPools()
        {
            List<GameObject> enemyPrefabs = new List<GameObject>();
            foreach (EnemyToPool enemy in m_pooledEnemies)
                enemyPrefabs.Add(enemy.enemyPrefab);

            m_enemyPool.CreatePool(enemyPrefabs, "Enemies");
            m_enemyProjectilesPool.CreatePool(m_pooledEnemyProjectiles, "Shooting Enemy Weapon Projectile");
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
            //little gymnastics because List<Structs> can't be directly modified
            
            EnemyToPool currentEnemy = m_pooledEnemies[(int)type];
            currentEnemy.canSpawn = value;
            m_pooledEnemies[(int)type] = currentEnemy;

        }

        public void CheatSpawnGroup(EEnemyTypes type, int amount)
        {
            Debug.Log(m_pooledEnemies[(int)type].enemyPrefab.name + "   " + amount);

            
            for (int i = 0; i < amount; i++)
            {
                Vector2 spawnWorldPos = FindValidEnemyRandomPos();
                m_enemyPool.Spawn(m_pooledEnemies[(int)type].enemyPrefab, spawnWorldPos);
            }
        }
        #endregion
    }
}
