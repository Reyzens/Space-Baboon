using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData m_weaponData;
        [SerializeField] protected ObjectPool m_pool;
        [SerializeField] protected float m_spawnDistance;
        [SerializeField] protected float m_attackSpeedScaling;
        [SerializeField] protected bool m_debugMode = false;

        protected float m_attackingCooldown = 0.0f;
        protected float m_attackSpeedModifier = 1.0f;
        protected int m_currentLevel = 1;
        protected bool m_isCollecting = false;

        protected void Awake()
        {
            m_pool.CreatePool(m_weaponData.projectilePrefab);
        }

        protected void Update()
        {
            if (m_isCollecting)
            {
                return;
            }

            if (m_attackingCooldown > m_weaponData.attackSpeed)
            {
                Attack();
                m_attackingCooldown = 0.0f;
            }
            m_attackingCooldown += Time.deltaTime * m_attackSpeedModifier;
        }

        protected void Attack()
        {
            Transform direction = GetTarget();

            //Vector2 directionWithDistance = direction.normalized * m_spawnDistance;
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
            var projectile = m_pool.Spawn(spawnPos);
            //Debug.Log("spawning  :" + projectile.GetComponent<Projectile>());

            projectile.GetComponent<Projectile>()?.Shoot(ref direction);
        }

        protected virtual Transform GetTarget()
        {
            return transform;
        }

        public void SetIsCollecting(bool value)
        {
            m_isCollecting = value;
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
