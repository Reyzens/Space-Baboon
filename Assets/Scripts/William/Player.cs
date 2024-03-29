using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{
    public class Player : MonoBehaviour , SpaceBaboon.IDamageable
    {
        [SerializeField]
        private PlayerData m_playerData;

        [SerializeField]
        private bool m_DebugMode = false;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory = new Dictionary<InteractableResource.EResourceType, int>();

        private Rigidbody2D m_playerRigidbody;
        private Transform m_playerTransform;

        private List<WeaponSystem.Weapon> m_equipedWeapon = new List<WeaponSystem.Weapon>();
        private List<WeaponSystem.Weapon> m_blockedWeapon = new List<WeaponSystem.Weapon>();
        
        [SerializeField]
        private float m_currentHealth;

        public bool m_alive;
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
            m_currentHealth = m_playerData.MaxHeatlh;
            m_alive = true;
            DictionaryInistalisation();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_currentHealth <= 0)
            {
                m_alive = false;
                SceneManager.LoadScene("SB_MainMenu");
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Structure"))
            {
                if (m_DebugMode) { Debug.Log("CollisionDetected with structure"); }

                collision.gameObject.GetComponent<ResourceDropPoint>().CollectResource(this);
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                OnDamageTaken(collision.gameObject.GetComponent<EnemySystem.Enemy>().GetDamage());
            }
           //if (collision.gameObject.CompareTag("Projectile"))
           //{
           //    OnDamageTaken(collision.gameObject.GetComponent<SpaceBaboon.Projectile>().GetDamage());
           //}
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
                //OnDamageTaken(10);
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

        public bool DropResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int amount)
        {
            if (m_collectibleInventory.ContainsKey(resourceType) && !(m_collectibleInventory[resourceType] < amount))
            {
                m_collectibleInventory[resourceType] -= amount;

                if (m_DebugMode) { Debug.Log(resourceType + " amount is : " + m_collectibleInventory[resourceType]); }
                return true;
            }

            return false;
        }
        #endregion

        public void OnDamageTaken(float values)
        {
            if (m_alive)
            {
                m_currentHealth -= values;
            }
            
            
        }

        public float GetCurrentHealth()
        {
            return m_currentHealth;
        }

        public int GetRessourceOne()
        {
            return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)0];
        }
        public int GetRessourceTwo()
        {
            return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)1];
        }
        public int GetRessourceTree()
        {
            return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)2];
        }
        private void DictionaryInistalisation()
        {
            for (int i = 0; i != (int)SpaceBaboon.InteractableResource.EResourceType.Count; i++)
            {
                m_collectibleInventory.Add((SpaceBaboon.InteractableResource.EResourceType)i,0);
            }        
        }
    }
}
