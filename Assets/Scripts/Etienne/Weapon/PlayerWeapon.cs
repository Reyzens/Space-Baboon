using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class PlayerWeapon : Weapon, IStatsEditable
    {
        [SerializeField] protected WeaponData m_weaponData;
        [SerializeField] protected GenericObjectPool m_pool = new GenericObjectPool();
        [SerializeField] protected bool m_debugMode = false;
        [SerializeField] float m_rotationAroundPlayerSpeed;
        [SerializeField] protected float m_MaxDistanceFromRotationTarget;
        [SerializeField] protected Transform m_owner;

        protected float m_attackingCooldown = 0.0f;
        protected float m_attackSpeedModifier = 1.0f;
        protected int m_currentLevel = 1;
        private bool m_weaponToggle = true;
        private Transform m_rotationTarget;

        //Collect Variables
        protected bool m_isCollecting = false;
        protected float m_currentCollectTimer;

        //Upgrade variables
        protected float m_rangeLevel = 0;
        protected float m_speedLevel = 0;
        protected float m_damageLevel = 0;
        protected float m_zoneLevel = 0;
        private const float ATTACKSPEEDLIMIT = 0.15f;

        protected float currentRange
        {
            get { return m_weaponData.maxRange + (m_rangeLevel * m_weaponData.m_rangeScaling); }
        }
        protected float currentSpeed
        {
            get { return m_weaponData.attackSpeed - (m_speedLevel * m_weaponData.m_speedScaling); }
        }
        protected float currentDamage
        {
            get { return m_weaponData.baseDamage + (m_damageLevel * m_weaponData.m_damageScaling); }
        }
        protected float currentZone
        {
            get { return m_weaponData.attackZone + (m_zoneLevel * m_weaponData.m_zoneScaling); }
        }

        protected virtual void Awake()
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(m_weaponData.projectilePrefab);

            m_pool.CreatePool(list, "Weapon Projectiles");
            m_rotationTarget = transform.parent;
        }
        protected virtual void Update()
        {
            if (!CheckIfCollecting())
            {
                AttackUpdate();
            }
            else
            {
                CollectUpdate();
            }
            RotateAroundPlayer();
        }
        public bool CheckIfCollecting()
        {
            return m_isCollecting;
        }
        private void AttackUpdate()
        {
            if (m_attackingCooldown < 0)
            {
                //Debug.Log("Attacking with weapon");
                Attack();
                m_attackingCooldown = currentSpeed;
            }
            m_attackingCooldown -= Time.deltaTime;
        }
        private void CollectUpdate()
        {
            m_currentCollectTimer -= Time.deltaTime;
            if (m_currentCollectTimer < 0)
            {
                m_isCollecting = false;
                transform.parent = m_owner;
                m_rotationTarget = m_owner;
            }
        }
        private void RotateAroundPlayer()
        {
            if (CanRotate())
            {
                transform.RotateAround(m_rotationTarget.position, Vector3.forward, m_rotationAroundPlayerSpeed * Time.deltaTime);
            }
            if (Vector2.Distance(transform.position, m_rotationTarget.position) > m_MaxDistanceFromRotationTarget)
            {
                MoveTowardTarget();
            }
        }
        private void MoveTowardTarget()
        {
            float distanceTowardTarget = Vector2.Distance(transform.position, m_rotationTarget.position);
            float scalingSpeed = Mathf.Lerp(0, m_rotationAroundPlayerSpeed, distanceTowardTarget / m_MaxDistanceFromRotationTarget);

            transform.position = Vector3.MoveTowards(transform.position, m_rotationTarget.position, scalingSpeed * Time.deltaTime);
        }
        private bool CanRotate()
        {
            if (m_rotationTarget != null && Vector2.Distance(transform.position, m_rotationTarget.position) < m_MaxDistanceFromRotationTarget)
            {
                return true;
            }

            return false;
        }
        public void ToggleWeapon()
        {
            m_weaponToggle = !m_weaponToggle;
            gameObject.SetActive(m_weaponToggle);
        }
        protected override void Attack()
        {
            Transform direction = GetTarget();
            if (direction != null)
            {
                Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
                var projectile = m_pool.Spawn(m_weaponData.projectilePrefab, spawnPos);
                //Debug.Log("spawning  :" + projectile.GetComponent<Projectile>());

                projectile.GetComponent<Projectile>()?.Shoot(direction, currentRange, currentZone, currentDamage, gameObject.transform);

                FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
                if (fxManager != null)
                {
                    fxManager.PlayAudio(m_weaponData.shootAudioCue);
                }
            }
        }
        protected virtual Transform GetTarget()
        {
            return null;
        }
        public float SetIsCollecting(bool value, Crafting.InteractableResource resourceToCollect)
        {
            m_isCollecting = value;
            m_rotationTarget = resourceToCollect.transform;
            m_currentCollectTimer = resourceToCollect.GetCollectTimer();

            //Set new parent or if resourceToCollect is null, return 0
            if (resourceToCollect != null)
            {
                transform.parent = resourceToCollect.transform;
            }
            else
            {
                return 0.0f;
            }
            return resourceToCollect.GetCollectTimer();
        }
        public WeaponData GetWeaponData() { return m_weaponData; }
        public override ScriptableObject GetData()
        {
            return m_weaponData;
        }
        #region Crafting
        public void Upgrade(Crafting.CraftingStation.EWeaponUpgrades upgrade)
        {
            if (m_debugMode)
            {
                Debug.Log("Weapon upgraded");
            }
            ApplyUpgrade(upgrade);
            m_currentLevel++;
        }

        private void ApplyUpgrade(Crafting.CraftingStation.EWeaponUpgrades upgrade)
        {
            switch (upgrade)
            {
                case Crafting.CraftingStation.EWeaponUpgrades.AttackZone:
                    m_zoneLevel++;
                    Debug.Log("Upgraded zone to " + m_zoneLevel);
                    break;
                case Crafting.CraftingStation.EWeaponUpgrades.AttackSpeed:
                    if (SpeedLimitReached())
                    {
                        ApplyUpgrade(Crafting.CraftingStation.EWeaponUpgrades.AttackDamage);
                        break;
                    }
                    m_speedLevel++;
                    Debug.Log("Upgraded attack speed to " + m_speedLevel);
                    break;
                case Crafting.CraftingStation.EWeaponUpgrades.AttackRange:
                    m_rangeLevel++;
                    Debug.Log("Upgraded range to " + m_rangeLevel);
                    break;
                case Crafting.CraftingStation.EWeaponUpgrades.AttackDamage:
                    m_damageLevel++;
                    Debug.Log("Upgraded damage to " + m_damageLevel);
                    break;
            }
        }
        private bool SpeedLimitReached()
        {
            if (currentSpeed < ATTACKSPEEDLIMIT)
            {
                Debug.Log("Attack speed limit reached");
                return true;
            }
            return false;
        }
        #endregion
    }
}
