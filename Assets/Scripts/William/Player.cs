using Cinemachine;
using SpaceBaboon.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{

    public class Player : Character, SpaceBaboon.IDamageable, IStatsEditable
    {
        //BaseVraiables
        private bool m_alive;
        private bool m_isDashing;
        private bool m_dashInputReceiver;
        private bool m_screenShake;
        private bool m_collectibleInRange;

        private float m_currentDashCDCounter;
        private float m_activeDashCD;
        private float m_activeDashCoolDown;
        private float m_dashCurveStrength;
        private float m_activeDashDuration;
        private float m_timestampedDash;

        //Weapons variables
        [SerializeField] private List<PlayerWeapon> m_weaponList = new List<PlayerWeapon>();
        private Dictionary<PlayerWeapon, SWeaponInventoryInfo> m_weaponInventory = new Dictionary<PlayerWeapon, SWeaponInventoryInfo>();

        class SWeaponInventoryInfo
        {
            public bool m_isReadyToCollect;
            public float m_collectTimer;

            public SWeaponInventoryInfo(bool collectStatus, float collectTimer = 0.0f)
            {
                m_isReadyToCollect = collectStatus;
                m_collectTimer = collectTimer;
            }
        }

        //private Vector2 m_movementDirection;
        private AnimationCurve m_dashCurve;
        private Color m_spriteRendererColor;

        private Dictionary<Crafting.InteractableResource.EResourceType, int> m_collectibleInventory;
        //private List<WeaponSystem.PlayerWeapon> m_equipedWeapon;
        //private List<WeaponSystem.PlayerWeapon> m_blockedWeapon;

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
        [SerializeField] private float m_collectRange;

        //Cheats related
        private bool m_isInvincible = false;


        //Unity Methods

        #region Unity
        private void Awake()
        {
            PlayerVariablesInitialization();
            FreezePlayerRotation();
            RegisterToGameManager();
        }

        private void Start()
        {
            DictionaryInistalisation();
            //So we don't forget our mistakes
            //PlayerVariablesInitialization();
            FreezePlayerRotation();
        }

        private void Update()
        {
            OnPlayerDeath();
            InventoryManagement();
        }

        private void FixedUpdate()
        {
            PlayerMovement();
            ActiveDashCdReduction();
            CheckForSpriteDirectionSwap(m_movementDirection);
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

            m_collectibleInventory = new Dictionary<Crafting.InteractableResource.EResourceType, int>();
            //m_equipedWeapon = new List<WeaponSystem.PlayerWeapon>();
            //m_blockedWeapon = new List<WeaponSystem.PlayerWeapon>();

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

        private void RegisterToGameManager()
        {
            GameManager.Instance.SetPlayer(this);
        }
        #endregion Initialiazation

        #region Events
        private void SubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashStartEvent += DashStart;
            InputHandler.instance.m_CollectResourceEvent += OnCollectResource;
        }

        private void UnsubscribeToInputEvent()
        {
            InputHandler.instance.m_MoveEvent -= Move;
            InputHandler.instance.m_DashStartEvent -= DashStart;
            InputHandler.instance.m_CollectResourceEvent -= OnCollectResource;
        }

        #endregion Events

        #region EventMethods

        protected override void Move(Vector2 values)
        {
            m_movementDirection = new Vector2(values.x, values.y).normalized;
        }
        public Vector2 GetPlayerDirection()
        {
            return m_movementDirection;
        }
        private void DashStart()
        {
            Debug.Log("Dash was called");
            if (m_activeDashCD <= 0.0f && m_movementDirection != Vector2.zero)
            {
                m_dashInputReceiver = true;
            }
        }
        private void OnCollectResource()
        {
            Debug.Log("OnCollectResource was called");
            if (m_collectibleInRange)
            {
                Crafting.InteractableResource resourceToCollect = SearchClosestResource();
                if (resourceToCollect != null)
                {
                    if (PickWeaponForCollect(resourceToCollect))
                    {
                        resourceToCollect.Collect(this);
                    }
                }
            }
        }
        #endregion EventMethods

        #region Collider
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Structure"))
            {
                if (m_DebugMode) { Debug.Log("CollisionDetected with structure"); }

                collision.gameObject.GetComponent<SpaceBaboon.Crafting.ResourceDropPoint>().CollectResource(this);
            }

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Resource"))
            {
                m_collectibleInRange = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Resource"))
            {
                m_collectibleInRange = false;
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

        private void PlayerMovement()
        {
            if (m_movementDirection != Vector2.zero)
            {
                m_rB.AddForce(m_movementDirection * AccelerationValue, ForceMode2D.Force);   //Etienne : change Acceleration from data.defaultAccel
                RegulateVelocity();
            }
            if (m_dashInputReceiver)
            {
                StartCoroutine(DashCoroutine());
                m_rB.AddForce(m_movementDirection * (m_dashCurveStrength * m_playerData.defaultDashAcceleration), ForceMode2D.Impulse);
            }
        }

        private void PlayerSpriteDirectionSwap()
        {
            if (m_movementDirection.x > 0)
            {
                m_renderer.flipX = false;
            }
            if (m_movementDirection.x < 0)
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

            FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
            if (fxManager != null)
            {
                fxManager.PlayAudio(FXSystem.EFXType.PlayerDash);
            }
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
            Debug.Log("player takes damage");

            // TODO change name of OnDamageTaken to AttackReceived
            // We could change the IDammageable interface to IAttackable
            // Player could eventually react to an attack here (for example momentarilly impervious, etc.)
            m_screenShake = true;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = m_screenShakeAmplitude;
            m_playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = m_screenShakeFrequency;
            m_renderer.material.color = Color.red;
            if (m_alive && !m_isInvincible)
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
        public void AddResource(Crafting.InteractableResource.EResourceType resourceType, int amount)
        {
            FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
            if (fxManager != null)
            {
                fxManager.PlayAudio(FXSystem.EFXType.CoinCollected);
            }


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
        private void InventoryManagement()
        {
            foreach (KeyValuePair<PlayerWeapon, SWeaponInventoryInfo> weapon in m_weaponInventory)
            {
                ReduceColletTimer(weapon.Value);
            }
        }
        private void ReduceColletTimer(SWeaponInventoryInfo collectTimerWeapon)
        {
            if (collectTimerWeapon.m_collectTimer > 0)
            { collectTimerWeapon.m_collectTimer -= Time.deltaTime; }

            if (collectTimerWeapon.m_collectTimer < 0)
            { collectTimerWeapon.m_isReadyToCollect = true; }
        }
        public bool DropResource(Crafting.InteractableResource.EResourceType resourceType, int amount)
        {
            if (m_collectibleInventory.ContainsKey(resourceType) && !(m_collectibleInventory[resourceType] < amount))
            {
                m_collectibleInventory[resourceType] -= amount;

                FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
                if (fxManager != null)
                {
                    fxManager.PlayAudio(FXSystem.EFXType.DroppingCoins);
                }


                if (m_DebugMode) { Debug.Log(resourceType + " amount is : " + m_collectibleInventory[resourceType]); }
                return true;
            }

            return false;
        }

        private Crafting.InteractableResource SearchClosestResource()
        {
            for (int i = 0; i < m_collectRange; i++)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, i);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Resource")
                    {
                        if (!collider.gameObject.GetComponent<Crafting.InteractableResource>().IsBeingCollected())
                        {
                            return collider.gameObject.GetComponent<Crafting.InteractableResource>();
                        }
                    }
                }
            }
            return null;
        }

        private bool PickWeaponForCollect(Crafting.InteractableResource resourceToCollect)
        {
            //Since melee weapon is index 0 of the enum and is the only one that can't collect, we start at index 1
            List<PlayerWeapon> availableWeapons = new List<PlayerWeapon>();

            //Check if weapon is available for collecting
            foreach (KeyValuePair<PlayerWeapon, SWeaponInventoryInfo> possibleWeapon in m_weaponInventory)
            {
                if (CheckIfWeaponIsAvailableForCollect(possibleWeapon.Value.m_isReadyToCollect, possibleWeapon.Key))
                {
                    availableWeapons.Add(possibleWeapon.Key);
                }
            }

            Debug.Log(availableWeapons.Count);

            //Pick a weapon at random
            if (availableWeapons.Count > 0)
            {
                //Choose weapon index
                int chosenWeaponindex = Random.Range(0, availableWeapons.Count);

                //Set chosen weapon to collecting and store the collect timer for later
                float newCollectTimer = availableWeapons[chosenWeaponindex].SetIsCollecting(true, resourceToCollect);

                List<PlayerWeapon> weaponsToUpdate = new List<PlayerWeapon>();

                foreach (KeyValuePair<PlayerWeapon, SWeaponInventoryInfo> weapon in m_weaponInventory)
                {
                    if (weapon.Key.GetWeaponData().weaponName == availableWeapons[chosenWeaponindex].GetWeaponData().weaponName)
                    {
                        weaponsToUpdate.Add(weapon.Key);
                    }
                }

                foreach (PlayerWeapon weapon in weaponsToUpdate)
                {
                    m_weaponInventory[weapon].m_isReadyToCollect = false;
                    m_weaponInventory[weapon].m_collectTimer = newCollectTimer;
                    Debug.Log(m_weaponInventory[weapon]);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckIfWeaponIsAvailableForCollect(bool collectingState, PlayerWeapon weaponTypeToCheck)
        {
            //Melee weapon is the only type that shouldn't ever collect
            return (collectingState && weaponTypeToCheck.GetWeaponData().weaponName != WeaponData.EPlayerWeaponType.Melee);
        }
        #endregion

        #region Gets

        public float GetCurrentHealth()
        {
            return m_activeHealth;
        }
        public int GetResources(int resourceType)
        {
            if (m_collectibleInventory.ContainsKey((Crafting.InteractableResource.EResourceType)resourceType))
            {
                return m_collectibleInventory[(Crafting.InteractableResource.EResourceType)resourceType];
            }
            else
            {
                return 0;
            }
        }
        private void DictionaryInistalisation()
        {
            //Initialize collectible inventory
            for (int i = 0; i != (int)Crafting.InteractableResource.EResourceType.Count; i++)
            {
                m_collectibleInventory.Add((Crafting.InteractableResource.EResourceType)i, 0);
            }

            //Initialize weapon inventory
            foreach (PlayerWeapon weapon in m_weaponList)
            {
                m_weaponInventory.Add(weapon, new SWeaponInventoryInfo(!weapon.CheckIfCollecting()));
            }
        }

        public List<WeaponSystem.PlayerWeapon> GetPlayerWeapons()
        {
            return m_weaponList;
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
            m_speedMultiplierCheat = value;
            Debug.Log("Max Velocity Mult : " + m_speedMultiplierCheat);
        }

        public void SetWeaponStatus(WeaponSystem.WeaponData.EPlayerWeaponType type, bool value)
        {
            foreach (KeyValuePair<PlayerWeapon, SWeaponInventoryInfo> weapon in m_weaponInventory)
            {
                if (weapon.Key.GetWeaponData().weaponName == type) { weapon.Key.SetIsCollecting(value, null); }
            }
        }

        public void KillPlayer()
        {
            m_activeHealth = 0;
        }

        #endregion

        public override ScriptableObject GetData()
        {
            return m_playerData;
        }
    }
}
