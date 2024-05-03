using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.Crafting
{
    public class InteractableResource : MonoBehaviour, IPoolable
    {
        //Serializable
        [SerializeField] private ResourceData m_resourceData;
        [SerializeField] private GameObject m_outline;

        //Private variables
        private bool m_isBeingCollected = false;
        private float m_currentCooldown = 0;
        private Player m_collectingPlayer;
        private GenericObjectPool m_shardPoolRef;
        private float shardsSpawnStrenght = 20f;
        private GameObject m_collectingWeapon;
        private bool m_isInCollectRange = false;
        private float m_collectRange;
        private Color m_rendereInitialColor;

        //Ipoolable variables
        private bool m_isActive = false;
        private ObjectPool m_parentPool;
        private SpriteRenderer m_renderer;
        private CircleCollider2D m_circleCollider;
        private CapsuleCollider2D m_capsuleCollider;

        //Static variables
        static Dictionary<EResourceType, ResourceData> Resources = new Dictionary<EResourceType, ResourceData>();

        public bool IsActive { get { return m_isActive; } }

        //Enums
        public enum EResourceType
        {
            Metal, Crystal, Technologie, Count
        }

        public static Dictionary<EResourceType, ResourceData> GetResourcesData()
        {
            return Resources;
        }
        //private Methods
        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            //Debug.Log(m_renderer);
            m_circleCollider = GetComponent<CircleCollider2D>();
            m_capsuleCollider = GetComponent<CapsuleCollider2D>();
            if (!Resources.ContainsKey(m_resourceData.m_resourceType))
            {
                Resources.Add(m_resourceData.m_resourceType, m_resourceData);
            }
        }
        private void Start()
        {
            m_outline.SetActive(false);
            m_collectRange = GameManager.Instance.Player.GetPlayerCollectRange();
            m_circleCollider.radius = m_collectRange;
            m_rendereInitialColor = m_renderer.color;
        }
        private void Update()
        {
            if (!m_isActive) return;

            if (m_currentCooldown > 0)
            {
                m_currentCooldown -= Time.deltaTime;
            }

            if (m_currentCooldown < 0) { FinishCollecting(); }
            if (m_isInCollectRange)
            {

            }
        }
        public void CollectableSizing(bool shouldGrow)
        {
            if (!m_isBeingCollected)
            {
                m_outline.gameObject.SetActive(shouldGrow);
            }
        }

        #region CollectingLogic
        public void Collect(Player collectingPlayer)
        {
            if (!m_isBeingCollected)
            {
                m_currentCooldown = m_resourceData.m_cooldownMax;
                m_isBeingCollected = true;
                m_collectingPlayer = collectingPlayer;
                m_renderer.color = Color.red;
                m_outline.gameObject.SetActive(false);

                FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
                if (fxManager != null)
                {
                    fxManager.PlayAudio(FXSystem.ESFXType.Mining);
                }
            }
        }
        private void FinishCollecting()
        {
            m_renderer.color = m_rendereInitialColor;

            FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
            if (fxManager != null)
            {
                fxManager.PlayAudio(FXSystem.ESFXType.MiningCompleted);
            }

            Vector2 direction;
            float angleBetweenShards = 360 / m_resourceData.m_resourceAmount;
            float spawnAngle;
            GameObject spawnedShard;

            for (int i = 0; i < m_resourceData.m_resourceAmount; i++)
            {
                spawnAngle = i * angleBetweenShards * Mathf.Deg2Rad;
                direction = new Vector2(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle));
                spawnedShard = m_shardPoolRef.Spawn(m_resourceData.m_shardPrefab, transform.position);
                spawnedShard.GetComponent<Crafting.ResourceShards>().Initialization(direction, Random.Range(0, shardsSpawnStrenght), m_collectingPlayer);
            }

            m_parentPool.UnSpawn(gameObject);
        }
        public GameObject GetResourceShardPrefab()
        {
            return m_resourceData.m_shardPrefab;
        }
        public bool IsBeingCollected()
        {
            return m_isBeingCollected;
        }
        public void SetCollectingWeapon(GameObject weapon)
        {
            m_collectingWeapon = weapon;
        }
        public float GetCollectTimer()
        {
            return m_resourceData.m_cooldownMax;
        }
        #endregion
        #region IPoolable
        public void Activate(Vector2 pos, ObjectPool pool)
        {
            ResetValues(pos);
            SetComponents(true);
            m_isActive = true;


            m_parentPool = pool;
        }
        public void Deactivate()
        {
            m_isActive = false;
            SetComponents(false);
        }
        private void ResetValues(Vector2 pos)
        {
            transform.position = pos;
            m_isBeingCollected = false;
            m_currentCooldown = 0;
        }
        private void SetComponents(bool value)
        {

            m_renderer.enabled = value;
            m_circleCollider.enabled = value;
            m_capsuleCollider.enabled = value;
        }
        public void SetShardPoolRef(GenericObjectPool shardPool)
        {
            m_shardPoolRef = shardPool;
        }
        public EResourceType GetResourceType()
        {
            return m_resourceData.m_resourceType;
        }
        #endregion
    }
}
