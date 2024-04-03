using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{
    public class Player : Character , SpaceBaboon.IDamageable
    {
        //BaseVraiables
        private bool m_alive;
        private int m_rotationlock;
        private float m_horizontal;
        private float m_vertical;
        private float m_currentDashCD;
        private float m_currentDashSpeed;
        private float m_currentDashDistance;
        private int m_currentDashStack;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory;
        private List<WeaponSystem.Weapon> m_equipedWeapon;
        private List<WeaponSystem.Weapon> m_blockedWeapon;
        
        //BonusVariables
        private float m_bonusDashCD;
        private float m_bonusDashSpeed;
        private float m_bonusDashDistance;
        private int m_bonusDashStack;
        
        
        //SerializeVraiables
        [SerializeField]private bool m_DebugMode;
        [SerializeField] private PlayerData m_playerData;
        
        
        
            
        //Unity Methods
        private void Awake()
        {
            PlayerVariablesInitialization();
            FreezePlayerRotation();
        }
        
        private void Start()
        {
            DictionaryInistalisation();
        }
        
        private void Update()
        {
            OnPlayerDeath();
        }

        private void FixedUpdate()
        {
            
        }

        private void OnDestroy()
        {
            UnsubscribeToInputEvent();
            
        }

        //Methods
        private void PlayerVariablesInitialization()
        {
            SubscribeToInputEvent();
                
            m_collectibleInventory = new Dictionary<InteractableResource.EResourceType, int>();
            m_equipedWeapon = new List<WeaponSystem.Weapon>();
            m_blockedWeapon = new List<WeaponSystem.Weapon>();
            
            m_characterRb = GetComponent<Rigidbody2D>();
            m_characterCollider = GetComponent<BoxCollider2D>();
            m_characterRenderer = GetComponent<Renderer>();

            m_currentHealth = m_playerData.DefaultBaseHeatlh;
            m_currentMovespeed = m_playerData.DefaultBaseMovespeed;
            m_currentAcceleration = m_playerData.DefaultBaseAcceleration;
            m_currentVelocity = m_playerData.DefaultBaseMaxVelocity;
            m_currentDashSpeed = m_playerData.DefaultDashSpeed;
            m_currentDashCD = m_playerData.DefaultDashCD;
            m_currentDashStack = m_playerData.DefaultDashStatck;
            m_currentDashDistance = m_playerData.DefaultDashDistance;
            
            m_rotationlock = 0;
            m_vertical = 0.0f;
            m_horizontal = 0.0f;
            
            m_bonusHealth = 0.0f;
            m_bonusMovespeed = 0.0f;
            m_bonusDashCD = 0.0f;
            m_bonusDashSpeed = 0.0f;
            m_bonusDashDistance = 0.0f;
            m_bonusDashStack = 0;
            
            
            m_alive = true;
            enabled = true;
        }
        
        private void SubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashEvent += Dash;
        }

        private void UnsubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent -= Move;
            InputHandler.instance.m_DashEvent -= Dash;
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

        private void FreezePlayerRotation()
        {
            m_characterRb.freezeRotation = enabled;
        }
        
        private void OnPlayerDeath()
        {
            if (m_currentHealth <= 0)
            {
                m_alive = false;
                SceneManager.LoadScene("SB_MainMenu");
            }
        }
        
        protected override void Move(Vector2 values) 
        {
            if (values == Vector2.zero)
            {
                m_characterRb.velocity = new Vector2(0, 0);
                m_characterRb.rotation = m_rotationlock;
                m_characterRb.angularVelocity = m_rotationlock;
                return;
            }
            
            m_horizontal = values.x;
            m_vertical = values.y;
            Vector2 movementVector2 = Vector2.zero;
            
            //Vector2 movementVector2 =  new Vector2(m_horizontal, m_vertical) * m_currentMovespeed;
            //m_characterRb.AddForce(movementVector2, ForceMode2D.Force);
            
            movementVector2.Set(values.x,values.y);
            m_characterRb.velocity = movementVector2 * m_currentMovespeed;

            if (movementVector2.magnitude > 0)
            {
                RegulateVelocity();
            }
            
            
            
            //aa
            
           //Vector3 playerPosition = m_players[0].transform.position;

           //Vector2 direction = (playerPosition - transform.position).normalized;
           //m_rb.AddForce(direction * m_enemyData.baseAcceleration /* + or * bonus */, ForceMode2D.Force);

           //if (direction.magnitude > 0)
           //    RegulateVelocity();
        }

        protected override void RegulateVelocity()
        {
            if (m_characterRb.velocity.magnitude > m_currentVelocity /* + or * bonus */)
            {
                m_characterRb.velocity = m_characterRb.velocity.normalized;
                m_characterRb.velocity *= m_currentVelocity /* + or * bonus */;
            }
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
