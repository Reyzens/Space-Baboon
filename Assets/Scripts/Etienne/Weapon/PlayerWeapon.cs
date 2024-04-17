using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public enum EPlayerWeaponType
    {
        Melee,
        FlameThrower,
        GrenadeLauncher,
        Shockwave,
        LaserBeam,
        Count
    }

    public class PlayerWeapon : Weapon, IStatsEditable
    {
        [SerializeField] protected WeaponData m_weaponData;
        [SerializeField] protected GenericObjectPool m_pool = new GenericObjectPool();
        [SerializeField] protected bool m_debugMode = false;
        [SerializeField] float m_rotationAroundPlayerSpeed;

        protected float m_attackingCooldown = 0.0f;
        protected float m_attackSpeedModifier = 1.0f;
        protected int m_currentLevel = 1;
        protected bool m_isCollecting = false;
        private bool m_weaponToggle = true;

        //Upgrade variables
        protected float m_rangeLevel = 1;
        protected float m_speedLevel = 1;
        protected float m_damageLevel = 1;
        protected float m_zoneLevel = 1;

        protected float currentRange
        {
            get { return m_weaponData.maxRange + (m_rangeLevel * m_weaponData.m_rangeScaling); }
        }
        //TODO add safety so it doesn't get to zero eventually
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
        }

        protected virtual void Update()
        {
            if (CheckIfCollecting())
            {
                AttackUpdate();
                RotateAroundPlayer();
            }
        }

        private bool CheckIfCollecting()
        {
            return !m_isCollecting;
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
        private void RotateAroundPlayer()
        {

            if (transform.parent != null)
            {
                transform.RotateAround(transform.parent.position, Vector3.forward, m_rotationAroundPlayerSpeed * Time.deltaTime);
            }
        }
        public void ToggleWeapon()
        {
            m_weaponToggle = !m_weaponToggle;
            gameObject.SetActive(m_weaponToggle);
        }
        protected override void Attack()
        {
            Transform direction = GetTarget();

            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            var projectile = m_pool.Spawn(m_weaponData.projectilePrefab, spawnPos);
            //Debug.Log("spawning  :" + projectile.GetComponent<Projectile>());

            projectile.GetComponent<Projectile>()?.Shoot(direction, currentRange, currentZone, currentDamage);
        }

        protected virtual Transform GetTarget()
        {
            return transform;
        }

        public void SetIsCollecting(bool value)
        {
            m_isCollecting = value;
        }
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
        #endregion

    }
}
