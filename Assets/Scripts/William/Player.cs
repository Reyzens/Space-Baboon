using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData m_playerData;

        [SerializeField]
        private bool m_DebugMode = false;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory = new Dictionary<InteractableResource.EResourceType, int>();

        private Rigidbody2D m_playerRigidbody;
        private Transform m_playerTransform;

        private List<Weapon> m_equipedWeapon = new List<Weapon>();
        private List<Weapon> m_blockedWeapon = new List<Weapon>();


        private float m_horizontal;
        private float m_vertical;
        private int m_rotationlock = 0;
        private float m_playerDashTimer;
        private float m_playerDashCDTimer;
        private float m_playerCurrentMovespeed;


        void Start()
        {
            enabled = true;
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashEvent += Dash;
            m_playerRigidbody = GetComponent<Rigidbody2D>();
            m_playerTransform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_DebugMode && collision.gameObject.tag == "Structure") { Debug.Log("CollisionDetected with structure"); }

            if (collision.gameObject.tag == "Structure")
            {
                collision.GetComponent<ResourceDropPoint>().CollectResource(this);
            }
        }

        private void Move(Vector2 values)
        {
            if (values == Vector2.zero)
            {
                enabled = true;
                m_playerRigidbody.velocity = new Vector2(0, 0);
                m_playerRigidbody.rotation = m_rotationlock;
                m_playerRigidbody.angularVelocity = m_rotationlock;
                return;
            }


            m_horizontal = values.x;
            m_vertical = values.y;

            m_playerRigidbody.velocity = new Vector2(m_horizontal, m_vertical) * m_playerData.Movespeed;
            m_playerRigidbody.freezeRotation = true;

            enabled = true;

        }

        private void Dash()
        {
            if (m_DebugMode)
            {
                Debug.Log("Dash");
            }
        }

        #region Crafting
        public void AddResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int amount)
        {
            if (!m_collectibleInventory.ContainsKey(resourceType))
            {
                m_collectibleInventory.Add(resourceType, amount);
            }
            else
            {
                m_collectibleInventory[resourceType] += amount;
            }

            if (m_DebugMode)
            {
                Debug.Log(resourceType + " amount is : " + m_collectibleInventory[resourceType]);
            }
        }

        public void DropResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int amount)
        {
            if (m_collectibleInventory.ContainsKey(resourceType) && !(m_collectibleInventory[resourceType] < amount))
            {
                m_collectibleInventory[resourceType] -= amount;

                if (m_DebugMode) { Debug.Log(resourceType + " amount is : " + m_collectibleInventory[resourceType]); }
            }
        }
        #endregion
    }
}
