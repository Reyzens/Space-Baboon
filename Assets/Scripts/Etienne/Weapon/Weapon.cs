using UnityEngine;

namespace SpaceBaboon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData m_weaponData;
        [SerializeField] private ObjectPool m_pool;
        [SerializeField] private float m_spawnDistance;
        [SerializeField] private bool m_debugMode = false;

        private float m_attackingCooldown = 0.0f;
        private int m_currentLevel = 1;
        private bool m_isCollecting = false;

        private void Awake()
        {
            m_pool.CreatePool(m_weaponData.projectilePrefab);
        }

        private void Update()
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
            m_attackingCooldown += Time.deltaTime;
        }

        private void Attack()
        {
            Vector2 direction = AimAtClosestEnemy();

            Vector2 directionWithDistance = direction.normalized * m_spawnDistance;
            Vector2 spawnPos = new Vector2(transform.position.x + directionWithDistance.x, transform.position.y + directionWithDistance.y);
            var projectile = m_pool.Spawn(spawnPos);
            //Debug.Log("spawning");

            projectile.GetComponent<Projectile>()?.Shoot(direction);
        }

        private Vector2 AimAtClosestEnemy()
        {

            for (int i = 0; i < m_weaponData.maxRange; i++)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, i);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Enemy")
                    {
                        Vector2 enemyPosition = collider.gameObject.transform.position;
                        Vector2 enemyDirection = enemyPosition - new Vector2(transform.position.x, transform.position.y);
                        return enemyDirection;
                    }
                }


            }

            //Didn't find an enemy
            return Vector2.up;
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
            }
            m_currentLevel++;
        }
        #endregion
    }
}
