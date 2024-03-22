using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class InteractableResource : MonoBehaviour
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

        //Static variables
        static Dictionary<EResourceType, ResourceData> Resources = new Dictionary<EResourceType, ResourceData>();

        //Enums
        public enum EResourceType
        {
            One, Two, Three, Count
        }

        //private Methods
        static Dictionary<EResourceType, ResourceData> GetResourcesData()
        {
            return Resources;
        }

        private void Update()
        {
            if (m_currentCooldown > 0)
            {
                m_currentCooldown -= Time.deltaTime;
            }

            if (m_currentCooldown < 0) { FinishCollecting(); }
        }

        void OnCollisionEnter2D(Collision2D collision)
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
            Destroy(gameObject);
        }
    }
}
