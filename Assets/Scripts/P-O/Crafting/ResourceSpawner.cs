using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SpaceBaboon
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_resourcesPrefab;
        //[SerializeField] private List<ObjectPool> m_resourcePool;
        //[SerializeField] private GameObject m_map;
        //[SerializeField] private float m_spawnRadiusFromScreenCorner = 0.0f;
        [SerializeField] private float m_spawningDelay = 0.0f;
        [SerializeField] private float m_spawningTimer = 0.0f;
        [SerializeField] private int m_poolSize;
        [SerializeField] private float m_mapBorderOffSet;
        [SerializeField] private List<Crafting.CraftingStation> m_craftingStationsInScene = new List<Crafting.CraftingStation>();

        //Tilemap refacto
        [SerializeField] private Tilemap m_tilemapRef;
        [SerializeField] private List<Vector3> m_spawnPositionsAvailable = new List<Vector3>();

        private Dictionary<GameObject, ObjectPool> m_resourceDictionary = new Dictionary<GameObject, ObjectPool>();
        private List<GameObject> m_resourceShardList = new List<GameObject>();
        private GenericObjectPool m_shardPool = new GenericObjectPool();
        //private SMapData m_mapData;


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

            //m_mapData = new SMapData(m_map);
        }

        private void Start()
        {
            m_spawningTimer = m_spawningDelay;

            foreach (KeyValuePair<GameObject, ObjectPool> resource in m_resourceDictionary)
            {
                resource.Value.SetPoolSize(m_poolSize);
                resource.Value.CreatePool(resource.Key);
            }
            GenerateGrid();
            m_shardPool.SetPoolStartingSize(m_poolSize);
            m_shardPool.CreatePool(m_resourceShardList, "Resource shard");
            SetupCraftingStationsIcon();

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

        #region ResourceSpawning
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

        private void CalculateSpawnPosition()
        {
            bool validPosFound = false;
            Vector2 spawnPosition = Vector2.zero;
            int whileiteration = 0;

            while (!validPosFound)
            {
                spawnPosition = RandomPosOnMap();

                validPosFound = CheckPosValidity(spawnPosition);

                //TODO fix this safety better
                whileiteration++;
                if (whileiteration > 50)
                {
                    Debug.Log("Reach max iteration of calculate spawn position");
                    validPosFound = true;
                }
            }

            SpawnResource(spawnPosition);
        }

        private Vector3 RandomPosOnMap()
        {
            int newPos = Random.Range(0, m_spawnPositionsAvailable.Count);
            return (Vector2)m_spawnPositionsAvailable[newPos] + new Vector2(0.5f, 0.5f);
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
            int whileiteration = 0;
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
                whileiteration++;
                if (whileiteration > 50)
                {
                    Debug.Log("Reach max iteration of SpawnResource");
                    canSpawn = true;
                    poolAmountTested = m_poolSize * m_resourcesPrefab.Count + 1;
                }
            }
        }
        #endregion
        #region Crafting
        private void SetupCraftingStationsIcon()
        {
            List<WeaponSystem.PlayerWeapon> weaponToSet = new List<WeaponSystem.PlayerWeapon>();
            foreach (WeaponSystem.PlayerWeapon playerWeapon in GameManager.Instance.Player.GetPlayerWeapons())
            {
                weaponToSet.Add(playerWeapon);
            }

            foreach (WeaponSystem.PlayerWeapon weapon in weaponToSet)
            {
                int CurrentStationIndex = Random.Range(0, m_craftingStationsInScene.Count);

                m_craftingStationsInScene[CurrentStationIndex].StationSetup(weapon);

                Crafting.CraftingStation craftingStationToRemove = m_craftingStationsInScene[CurrentStationIndex];
                m_craftingStationsInScene.Remove(craftingStationToRemove);
            }
        }
        #endregion
    }
}
