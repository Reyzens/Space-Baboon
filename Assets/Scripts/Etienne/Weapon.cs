using UnityEngine;

namespace SpaceBaboon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponData m_weaponData;

        private float m_attackingCooldown = 0.0f;
        private int m_currentLevel = 1;
        private bool m_isCollecting = false;



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
            Vector2 direction = transform.position - transform.root.position;
            var projectile = Instantiate(m_weaponData.projectilePrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            projectile.GetComponent<Projectile>()?.Shoot(direction);
        }
    }
}
