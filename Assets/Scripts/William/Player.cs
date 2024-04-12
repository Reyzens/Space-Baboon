using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{
    public class Player : Character, SpaceBaboon.IDamageable
    {
        //BaseVraiables
        private bool m_alive;
        private bool m_isDashing;
        private bool m_dashInputReceiver;
        private bool m_screenShake;

        private float m_currentDashCDCounter;
        private float m_activeDashCD;
        private float m_activeDashCoolDown;
        private float m_dashCurveStrength;
        private float m_activeDashDuration;
        private float m_timestampedDash;

        private Vector2 m_playerDirectionVector2;
        private AnimationCurve m_dashCurve;
        private Color m_spriteRendererColor;

        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory;
        private List<WeaponSystem.PlayerWeapon> m_equipedWeapon;
        private List<WeaponSystem.PlayerWeapon> m_blockedWeapon;

        //BonusVariables
        private float m_bonusDashCD;
        private float m_bonusDashSpeed;
        private float m_bonusDashDistance;
        private int m_bonusDashStack;


        //SerializeVraiables
        [SerializeField] private bool m_DebugMode;
        [SerializeField] private PlayerData m_playerData;
        [SerializeField] private CinemachineVirtualCamera m_playerCam;
        [SerializeField] private GameObject m_dahsTrail;
        [SerializeField] private float m_screenShakeAmplitude = 5.0f;
        [SerializeField] private float m_screenShakeFrequency = 1.0f;

        //Cheats related
        private bool m_isInvincible = false;
        private float m_maxVelocityMultiplierCheat = 1.0f;


        //Unity Methods

        #region Unity
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
            PlayerDamageTakenScreenShake();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputEvent();
        }
        #endregion

        //Methods

        #region Initialiazation
        private void PlayerVariablesInitialization()
        {
            InputHandler.instance.m_Input.Enable();
            SubscribeToInputEvent();

            m_collectibleInventory = new Dictionary<InteractableResource.EResourceType, int>();
            m_equipedWeapon = new List<WeaponSystem.PlayerWeapon>();
            m_blockedWeapon = new List<WeaponSystem.PlayerWeapon>();

            m_rB = GetComponent<Rigidbody2D>();
            m_collider = GetComponent<BoxCollider2D>();
            m_renderer = GetComponent<SpriteRenderer>();
            m_playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            m_spriteRendererColor = m_renderer.color;


            m_activeHealth = m_playerData.defaultHealth + m_bonusHealth;
            m_activeDashCoolDown = m_playerData.defaultDashCd;
            m_activeDashDuration = m_playerData.defaultDashDuration;
            m_dashCurve = m_playerData.defaultDashCurve;

            m_bonusHealth = 0.0f;
            m_bonusDashCD = 0.0f;
            m_bonusDashSpeed = 0.0f;
            m_bonusDashDistance = 0.0f;
            m_bonusDashStack = 0;

            m_alive = true;
            enabled = true;
            m_isDashing = false;
            m_activeDashCD = 0.0f;
            m_dahsTrail.SetActive(false);
            m_screenShake = false;
            m_dashCurveStrength = 0.0f;
            m_timestampedDash = 0.0f;
        }

        #endregion Initialiazation

        #region Events
        private void SubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashStartEvent += DashStart;
        }

        private void UnsubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent -= Move;
            InputHandler.instance.m_DashStartEvent -= DashStart;
        }

        #endregion Events

        #region EventMethods

        protected override void Move(Vector2 values)
        {
            m_playerDirectionVector2 = new Vector2(values.x, values.y).normalized;
        }
        public Vector2 GetPlayerDirection()
        {
            return m_playerDirectionVector2;
        }
        private void DashStart()
        {
            if (m_activeDashCD <= 0.0f && m_playerDirectionVector2 != Vector2.zero)
            {
                m_dashInputReceiver = true;
            }
        }
        #endregion EventMethods

        #region Collider
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

        #endregion Collider

        #region PlayerMethods
        private void FreezePlayerRotation()
        {
            m_rB.freezeRotation = enabled;
        }

        private void OnPlayerDeath()
        {
            if (m_activeHealth <= 0 || m_alive == false)
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

        protected override void RegulateVelocity()
        {
            if (m_rB.velocity.magnitude > m_playerData.defaultMaxVelocity)
            {
                m_rB.velocity = m_rB.velocity.normalized;
                m_rB.velocity *= m_playerData.defaultMaxVelocity;
            }
        }

        private void PlayerMovement()
        {
            if (m_playerDirectionVector2 != Vector2.zero)
            {
                m_rB.AddForce(m_playerDirectionVector2 * m_playerData.defaultAcceleration, ForceMode2D.Force);
                RegulateVelocity();
            }
            if (m_dashInputReceiver)
            {
                StartCoroutine(DashCoroutine());
                m_rB.AddForce(m_playerDirectionVector2 * (m_dashCurveStrength * m_playerData.defaultDashAcceleration), ForceMode2D.Impulse);
            }
        }

        private void PlayerSpriteDirectionSwap()
        {
            if (m_playerDirectionVector2.x > 0)
            {
                m_renderer.flipX = false;
            }
            if (m_playerDirectionVector2.x < 0)
            {
                m_renderer.flipX = true;
            }
        }

        private void AfterDashCoroutine()
        {
            m_activeDashCD = m_activeDashCoolDown;
            m_renderer.material.color = Color.Lerp(m_renderer.material.color, m_spriteRendererColor, 0.2f);
            m_dahsTrail.SetActive(false);
            m_isDashing = false;
            m_dashInputReceiver = false;
            m_rB.GameObject().layer = LayerMask.NameToLayer("Player");
            //Physics2D.IgnoreLayerCollision(LayerMask.GetMask("Player"),LayerMask.GetMask("Enemy"),false);
        }

        private void BeforeDashCoroutine()
        {
            m_isDashing = true;
            m_timestampedDash = 0.0f;
            m_renderer.material.color = new Color(1f, 1f, 1f, 0.2f);
        }

        private void PlayerDamageTakenScreenShake()
        {
            if (m_screenShake)
            {
                StartCoroutine(PlayerDamageTakenScreenShakeCoroutine());
            }
        }

        public override void OnDamageTaken(float damage)
        {
            // TODO change name of OnDamageTaken to AttackReceived
            // We could change the IDammageable interface to IAttackable
            // Player could eventually react to an attack here (for example momentarilly impervious, etc.)
            m_screenShake = true;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = m_screenShakeAmplitude;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = m_screenShakeFrequency;
            m_renderer.material.color = Color.red;
            if (m_alive && !m_isInvincible) // TODO if statement may not be useful, if so remove it
                m_activeHealth -= damage;
        }

        #endregion PlayerMethods

        #region PlayerCoroutine
        private IEnumerator DashCoroutine()
        {
            BeforeDashCoroutine();
            while (m_timestampedDash < m_activeDashDuration)
            {
                m_timestampedDash += Time.deltaTime;
                float dashCurvePosition = m_timestampedDash / m_activeDashDuration;
                m_dashCurveStrength = m_dashCurve.Evaluate(dashCurvePosition);
                m_rB.GameObject().layer = LayerMask.NameToLayer("ImmunityDash");
                //Physics2D.IgnoreLayerCollision(LayerMask.GetMask("Player"),LayerMask.GetMask("Enemy"),true);
                m_dahsTrail.SetActive(true);
                yield return null;
            }
            AfterDashCoroutine();
        }

        private IEnumerator PlayerDamageTakenScreenShakeCoroutine()
        {
            m_screenShake = false;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
            m_renderer.material.color = m_spriteRendererColor;
            yield return new WaitForSeconds(0.5f);
        }

        #endregion PlayerCoroutine

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

        #region Gets

        public float GetCurrentHealth()
        {
            return m_activeHealth;
        }

        public int GetRessourceOne()
        {
            if (m_collectibleInventory.ContainsKey((SpaceBaboon.InteractableResource.EResourceType)0))
            {
                return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)0];
            }
            else
            {
                return 0;
            }
        }
        public int GetRessourceTwo()
        {
            if (m_collectibleInventory.ContainsKey((SpaceBaboon.InteractableResource.EResourceType)1))
            {
                return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)1];
            }
            else
            {
                return 0;
            }
        }
        public int GetRessourceThree()
        {
            if (m_collectibleInventory.ContainsKey((SpaceBaboon.InteractableResource.EResourceType)2))
            {
                return m_collectibleInventory[(SpaceBaboon.InteractableResource.EResourceType)2];
            }
            else
            {
                return 0;
            }
        }
        private void DictionaryInistalisation()
        {
            for (int i = 0; i != (int)SpaceBaboon.InteractableResource.EResourceType.Count; i++)
            {
                m_collectibleInventory.Add((SpaceBaboon.InteractableResource.EResourceType)i, 0);
            }
        }

        #endregion Gets

        #region Cheats
        public void SetIsInvincible(bool value)
        {
            m_isInvincible = value;
            Debug.Log("Invincibility is : " + m_isInvincible);
        }

        public void SetCurrentHealthToMax()
        {
            m_activeHealth = m_playerData.defaultHealth;
        }

        public void SetSpeedWithMultiplier(float value)
        {
            m_maxVelocityMultiplierCheat = value;
            Debug.Log("Max Velocity Mult : " + m_maxVelocityMultiplierCheat);
        }

        #endregion
    }
}
