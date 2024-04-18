using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class InteractableResource : MonoBehaviour, IPoolable
    {
        //Serializable
        [SerializeField]
        private ResourceData m_resourceData;
        [SerializeField]
        private bool m_DebugMode;

        //Private variables
        private bool m_isBeingCollected = false;
        private float m_currentCooldown = 0;
        private Player m_collectingPlayer;

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

        //private Methods
        static Dictionary<EResourceType, ResourceData> GetResourcesData()
        {
            return Resources;
        }
        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            //Debug.Log(m_renderer);
            m_circleCollider = GetComponent<CircleCollider2D>();
            m_capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
        private void Start()
        {

        }
        private void Update()
        {
            if (!m_isActive) return;

            if (m_currentCooldown > 0)
            {
                m_currentCooldown -= Time.deltaTime;
            }

            if (m_currentCooldown < 0) { FinishCollecting(); }
        }

        #region CollectingLogic
        void OnCollisionStay2D(Collision2D collision)
        {
            if (m_DebugMode && collision.gameObject.tag == "Player") { Debug.Log("CollisionDetected with player"); }

            if (collision.gameObject.tag == "Player" && !m_isBeingCollected)
            {
                Collect(collision.gameObject.GetComponent<Player>());
            }
        }
        private void Collect(Player collectingPlayer)
        {
            if (!m_isBeingCollected)
            {
                m_currentCooldown = m_resourceData.m_cooldownMax;
                m_isBeingCollected = true;
                m_collectingPlayer = collectingPlayer;
            }
        }

        private void FinishCollecting()
        {
            if (m_DebugMode) { Debug.Log("FinishedCollecting :" + this); m_currentCooldown = 0.0f; }
            m_collectingPlayer.AddResource(m_resourceData.m_resourceType, m_resourceData.m_resourceAmount);
            m_parentPool.UnSpawn(gameObject);
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
        #endregion
    }
}
