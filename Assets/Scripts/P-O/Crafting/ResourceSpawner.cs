using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_resourcesPrefab;
        //[SerializeField] private List<ObjectPool> m_resourcePool;
        [SerializeField] private GameObject m_map;
        //[SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private float m_spawningTimer = 0.0f;
        [SerializeField] private int m_poolSize;
        [SerializeField] private float m_mapBorderOffSet;

        private Dictionary<GameObject, ObjectPool> m_resourceDictionary = new Dictionary<GameObject, ObjectPool>();
        private List<GameObject> m_resourceShardList = new List<GameObject>();
        private GenericObjectPool m_shardPool = new GenericObjectPool();
        private SMapData m_mapData;


        private void Awake()
        {
            foreach (GameObject resource in m_resourcesPrefab)
            {
                if (!m_resourceDictionary.ContainsKey(resource))
                {
                    m_resourceDictionary.Add(resource, new ObjectPool());
                    m_resourceShardList.Add(resource.GetComponent<Crafting.InteractableResource>().GetResourceShardPrefab());
                }
            }

            m_mapData = new SMapData(m_map);
        }

        private void Start()
        {
            m_spawningTimer = m_spawningDelay;

            foreach (KeyValuePair<GameObject, ObjectPool> resource in m_resourceDictionary)
            {
                resource.Value.SetPoolSize(m_poolSize);
                resource.Value.CreatePool(resource.Key);
            }

            m_shardPool.SetPoolStartingSize(m_poolSize);
            m_shardPool.CreatePool(m_resourceShardList, "Resource shard");
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.C)) // Press C to add 1 enemy at will (for testing)
            //    CalculateSpawnPosition();

            m_spawningTimer -= Time.deltaTime;

            if (m_spawningTimer <= 0.0f)
            {
                m_spawningTimer = m_spawningDelay;
                CalculateSpawnPosition();
            }
        }

        private void CalculateSpawnPosition()
        {
            bool validPosFound = false;
            Vector2 spawnPosition = Vector2.zero;

            while (!validPosFound)
            {
                spawnPosition = RandomPosOnMap();

                validPosFound = CheckPosValidity(spawnPosition);
            }

            SpawnResource(spawnPosition);
        }

        private Vector2 RandomPosOnMap()
        {
            //Get map width and heigth
            float x = m_mapData.GetMapWidth();
            float y = m_mapData.GetMapHeight();

            //Get a position on the map
            Vector2 pos = new Vector2(Random.Range(0, x) - m_mapData.GetMapHalfWidth(), Random.Range(0, y) - m_mapData.GetMapHalfHeight());

            //Apply map border offset to make sure it's not too close to the map edge
            if (pos.x < 0)
            {
                pos.x += m_mapBorderOffSet;
            }
            else
            {
                pos.x -= m_mapBorderOffSet;
            }

            if (pos.y < 0)
            {
                pos.y += m_mapBorderOffSet;
            }
            else
            {
                pos.y -= m_mapBorderOffSet;
            }

            return pos;
        }

        private bool CheckPosValidity(Vector2 positionToTest)
        {
            //Check if the position is valid
            Collider2D colliderOnPos = Physics2D.OverlapPoint(positionToTest);

            if (colliderOnPos != null) { return false; }

            return true;
        }
        private void SpawnResource(Vector2 spawnPosition)
        {
            //Choose resource to spawn
            bool canSpawn = true;
            int poolAmountTested = 0;
            while (canSpawn && poolAmountTested < m_poolSize * m_resourcesPrefab.Count)
            {
                int resourceIndex = Random.Range(0, m_resourcesPrefab.Count);
                if (m_resourceDictionary[m_resourcesPrefab[resourceIndex]].GetPoolQueueSize() != 0)
                {
                    var spawnedResource = m_resourceDictionary[m_resourcesPrefab[resourceIndex]].Spawn(spawnPosition);
                    spawnedResource.GetComponent<Crafting.InteractableResource>().SetShardPoolRef(m_shardPool);
                    canSpawn = false;
                }
                else
                {
                    poolAmountTested += m_poolSize;
                    canSpawn = true;
                }
            }
        }

        private struct SMapData
        {
            float m_mapWidth;
            float m_mapHeight;
            Vector2 m_mapOffset;
            float m_mapHalfWidth;
            float m_mapHalfHeight;

            public SMapData(GameObject map)
            {
                m_mapWidth = map.transform.localScale.x;
                m_mapHeight = map.transform.localScale.y;
                m_mapHalfHeight = m_mapHeight * 0.5f;
                m_mapHalfWidth = m_mapWidth * 0.5f;
                m_mapOffset = new Vector2(map.transform.position.x, map.transform.position.y);
            }

            public float GetMapWidth() { return m_mapWidth; }
            public float GetMapHeight() { return m_mapHeight; }
            public Vector2 GetMapOffset() { return m_mapOffset; }
            public float GetMapHalfWidth() { return m_mapHalfWidth; }
            public float GetMapHalfHeight() { return m_mapHalfHeight; }
        }
    }
}
