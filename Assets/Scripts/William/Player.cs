using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        private bool m_dashInputReceiver;
        private float m_currentDashCDCounter;
        private Vector2 m_playerInitialPosition;
        private Vector2 m_playerEndingPosition;
        private float m_activeDashCD;
        private AnimationCurve m_dashCurve;
        private float m_currentDashCD;
        private float m_currentDashSpeed;
        private Color m_spriteRendererColor;
        [SerializeField]
        private GameObject m_dahsTrail;
        [SerializeField] private bool m_isDashing;
        private float m_currentDashDuration;
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
            PlayerVariablesInitialization();
            FreezePlayerRotation();
        }

        private void Update()
        {
            OnPlayerDeath();
            
        }

        private void FixedUpdate()
        {
            PlayerMovement();
            ActiveDashCdReduction();
            PlayerSpriteDirectionSwap();
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
            m_characterRenderer = GetComponent<SpriteRenderer>();
            
            
           
            

            m_currentHealth = m_playerData.defaultHealth;
            m_currentVelocity = m_playerData.defaultMaxVelocity;
            m_currentDashSpeed = m_playerData.defaultDashVelocity;
            m_currentDashCD = m_playerData.defaultDashCd;
            m_currentDashDuration = m_playerData.defaultDashDuration;
            m_dashCurve = m_playerData.defaultDashCurve;


            m_rotationlock = 0;
            m_vertical = 0.0f;
            m_horizontal = 0.0f;

            m_bonusHealth = 0.0f;
            m_bonusVelocity = 0.0f;
            m_bonusDashCD = 0.0f;
            m_bonusDashSpeed = 0.0f;
            m_bonusDashDistance = 0.0f;
            m_bonusDashStack = 0;

            m_alive = true;
            enabled = true;
            m_isDashing = false;
            m_activeDashCD = 0.0f;
            m_dahsTrail.SetActive(false);


        }

        private void SubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashStartEvent += DashStart;
            //.instance.m_DashEndEvent += DashEnd;
        }

        private void UnsubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent -= Move;
            InputHandler.instance.m_DashStartEvent -= DashStart;
            //InputHandler.instance.m_DashEndEvent -= DashEnd;
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

        private void ActiveDashCdReduction()
        {
            if (m_activeDashCD > 0.0f)
            {
                m_activeDashCD -= Time.deltaTime;
                
            }
        }

        protected override void Move(Vector2 values)
        {
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

        private void PlayerMovement()
        {
            if (m_destination != Vector2.zero) // Check if there's any movement input
            {
                
                m_characterRb.AddForce(m_destination * m_currentVelocity, ForceMode2D.Force);
                RegulateVelocity();
            }

            if (m_dashInputReceiver && m_activeDashCD <= 0.0f && m_destination != Vector2.zero)
            {
                StartCoroutine(DashCoroutine());
            }
            
        }

        private void DashStart()
        {  
            if(m_activeDashCD <= 0.0f)
            {
                m_dashInputReceiver = true;
            }
        }

        private void PlayerSpriteDirectionSwap()
        {
            if (m_destination.x > 0)
            {
                m_characterRenderer.flipX = false;
            }
            if (m_destination.x < 0)
            {
                m_characterRenderer.flipX = true;
            }
        }
        
        private IEnumerator DashCoroutine()
        {
            m_isDashing = true;
            Color savedColor = m_characterRenderer.color;
            float timestamped = 0.0f;
            m_characterRenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
            while (timestamped < m_currentDashDuration)
            {
                timestamped += Time.deltaTime;
                float dashCurvePosition = timestamped / m_currentDashDuration;
                float dashCurveStrength = m_dashCurve.Evaluate(dashCurvePosition);
                m_characterRb.AddForce(m_destination * (dashCurveStrength * m_currentDashSpeed), ForceMode2D.Force);
                this.gameObject.layer = LayerMask.NameToLayer("ImmunityDash");
                m_dahsTrail.SetActive(true);
                yield return null;
            }
            m_activeDashCD = m_currentDashCD;
            m_characterRenderer.material.color = Color.Lerp(m_characterRenderer.material.color,savedColor,0.2f);
            m_dahsTrail.SetActive(false);
            m_isDashing = false;
            m_dashInputReceiver = false;
            this.gameObject.layer = LayerMask.NameToLayer("Player");
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
            // TODO change name of OnDamageTaken to AttackReceived
            // We could change the IDammageable interface to IAttackable
            // Player could eventually react to an attack here (for example momentarilly impervious, etc.)

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
