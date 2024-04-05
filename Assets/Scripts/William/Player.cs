using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

namespace SpaceBaboon
{
    public class Player : Character, SpaceBaboon.IDamageable
    {
        //BaseVraiables
        private bool m_alive;
        private int m_rotationlock;
        private float m_horizontal;
        private float m_vertical;
        private Vector2 m_destination;
        [SerializeField] private float m_currentDashCD;
        [SerializeField] private float m_currentDashSpeed;
        [SerializeField] private float m_currentDashDistance;
        [SerializeField] private int m_currentDashStack;
        [SerializeField] private bool m_isDashind;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory;
        private List<WeaponSystem.Weapon> m_equipedWeapon;
        private List<WeaponSystem.Weapon> m_blockedWeapon;

        //BonusVariables
        private float m_bonusDashCD;
        private float m_bonusDashSpeed;
        private float m_bonusDashDistance;
        private int m_bonusDashStack;


        //SerializeVraiables
        [SerializeField] private bool m_DebugMode;
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
            if (m_destination != Vector2.zero) // Check if there's any movement input
            {
                m_characterRb.AddForce(m_destination * m_playerData.defaultAcceleration, ForceMode2D.Force);
                //Debug.Log("destination value.x = " + m_destination.x);
                //Debug.Log("destination value.y = " + m_destination.y);
                //RegulateVelocity();
                //Debug.Log("destination value.x after Regulate = " + m_destination.x);
                //Debug.Log("destination value.y after Regulate = " + m_destination.y);
            }

            RegulateVelocity();
            //if (m_destination is { x: 0, y: 0 })
            //{
            //    m_characterRb.velocity = Vector2.zero;
            //}

        }

        private void OnDestroy()
        {
            UnsubscribeToInputEvent();

        }

        //Methods
        private void PlayerVariablesInitialization()
        {
            InputHandler.instance.m_Input.Enable();
            SubscribeToInputEvent();


            m_collectibleInventory = new Dictionary<InteractableResource.EResourceType, int>();
            m_equipedWeapon = new List<WeaponSystem.Weapon>();
            m_blockedWeapon = new List<WeaponSystem.Weapon>();

            m_characterRb = GetComponent<Rigidbody2D>();
            m_characterCollider = GetComponent<BoxCollider2D>();
            m_characterRenderer = GetComponent<Renderer>();

            m_currentHealth = m_playerData.defaultHeatlh;
            m_currentMovespeed = m_playerData.defaultMovespeed;
            m_currentAcceleration = m_playerData.defaultAcceleration;
            m_currentVelocity = m_playerData.defaultMaxVelocity;
            m_currentDashSpeed = m_playerData.defaultDashSpeed;
            m_currentDashCD = m_playerData.defaultDashCD;
            m_currentDashStack = m_playerData.defaultDashStatck;
            m_currentDashDistance = m_playerData.defaultDashDistance;

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
            m_isDashind = false;
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

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    //if (collision.gameObject.CompareTag("Projectile"))
        //    //{
        //    //    OnDamageTaken(collision.gameObject.GetComponent<SpaceBaboon.Projectile>().GetDamage());
        //    //}
        //}

        private void FreezePlayerRotation()
        {
            m_characterRb.freezeRotation = enabled;
        }

        private void OnPlayerDeath()
        {
            if (m_currentHealth <= 0 || m_alive == false)
            {
                m_alive = false;
                InputHandler.instance.m_Input.Disable();
                SceneManager.LoadScene("SB_MainMenu");
            }
        }

        protected override void Move(Vector2 values)
        {
            //Debug.Log("value.x = " + values.x);
            //Debug.Log("value.y = " + values.y);
            m_destination = new Vector2(values.x, values.y).normalized;
        }

        protected override void RegulateVelocity()
        {
            if (m_characterRb.velocity.magnitude > m_playerData.defaultMaxVelocity /* + or * bonus */)
            {
                m_characterRb.velocity = m_characterRb.velocity.normalized;
                m_characterRb.velocity *= m_currentVelocity /* + or * bonus */;
            }
        }

        private void Dash()
        {
            StartCoroutine(DashCouritine());
        }

        private IEnumerator DashCouritine()
        {
            if (m_currentDashStack >= 1)
            {
                m_isDashind = true;
                m_characterRb.velocity = new Vector2(m_horizontal * m_currentDashSpeed, m_vertical * m_currentDashSpeed);
                yield return new WaitForSeconds(m_currentDashCD);
                m_isDashind = false;
            }
            //if (m_DebugMode)
            //{
            //    //OnDamageTaken(10);
            //    m_alive = false;
            //    Debug.Log("Dash");
            //}
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

        public override void OnDamageTaken(float damage)
        {
            if (m_alive) // TODO if statement may not be useful, if so remove it
                m_currentHealth -= damage;
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
                m_collectibleInventory.Add((SpaceBaboon.InteractableResource.EResourceType)i, 0);
            }
        }


    }
}
