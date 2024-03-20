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
            //Vector2 direction = transform.position - transform.root.position;
            Vector2 direction = AimAtClosestEnemy();
            var projectile = Instantiate(m_weaponData.projectilePrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

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
    }
}
