using SpaceBaboon.PoolingSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class PlayerWeapon : Weapon
    {
        [SerializeField] protected WeaponData m_weaponData;
        [SerializeField] private GenericObjectPool m_pool = new GenericObjectPool();
        [SerializeField] protected float m_attackSpeedScaling;
        [SerializeField] protected bool m_debugMode = false;
        [SerializeField] float m_rotationAroundPlayerSpeed;

        protected float m_attackingCooldown = 0.0f;
        protected float m_attackSpeedModifier = 1.0f;
        protected int m_currentLevel = 1;
        protected bool m_isCollecting = false;
        private bool m_weaponToggle = true;

        protected virtual void Awake()
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(m_weaponData.projectilePrefab);

            m_pool.CreatePool(list, "Weapon Projectiles");
        }

        protected void Update()
        {
            if (m_isCollecting)
            {
                return;
            }

            if (m_attackingCooldown > m_weaponData.attackSpeed)
            {
                //Debug.Log("Attacking with weapon");
                Attack();
                m_attackingCooldown = 0.0f;
            }
            m_attackingCooldown += Time.deltaTime * m_attackSpeedModifier;
            RotateAroundPlayer();
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

            projectile.GetComponent<Projectile>()?.Shoot(ref direction, m_weaponData.maxRange, m_weaponData.attackZone);
        }

        protected virtual Transform GetTarget()
        {
            return transform;
        }

        public void SetIsCollecting(bool value)
        {
            m_isCollecting = value;
        }

        public float GetWeaponRange()
        {
            return m_weaponData.maxRange;
        }

        #region Crafting
        public void Upgrade()
        {
            if (m_debugMode)
            {
                Debug.Log("Weapon upgraded");
                m_attackSpeedModifier += m_attackSpeedScaling;
            }
            m_currentLevel++;
        }
        #endregion
    }
}
