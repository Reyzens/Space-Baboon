using UnityEngine;

namespace SpaceBaboon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData m_projectileData;

        private Vector2 m_direction;
        private float m_lifetime = 0.0f;
        private bool m_isActive = false;


        private void Update()
        {
            if (!m_isActive)
            {
                return;
            }

            if (m_lifetime > m_projectileData.maxLifetime)
            {
                //m_isActive = false;
                Destroy(gameObject);
            }
            m_lifetime += Time.deltaTime;

            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime);
        }

        public void Shoot(Vector2 direction)
        {
            m_isActive = true;
            m_direction = direction.normalized;
        }
    }
}
