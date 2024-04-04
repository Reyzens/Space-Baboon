using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon
{
    public class GrenadeLauncherProjectile : Projectile, IExplodable
    {
        //Private variables
        [SerializeField] private float m_curvatureStrength;
        [SerializeField] private Vector2 m_curvatureDirection = Vector2.up;
        private Vector2 m_initialPosition;
        private Vector2 m_centerPoint;
        // Rotation speed in degrees per second
        [SerializeField] private float rotationSpeed = 90f;
        public float m_currentExplodingTimer => throw new System.NotImplementedException();

        public float m_maxExplodingTime => throw new System.NotImplementedException();

        public float m_maxExplodingRadius => throw new System.NotImplementedException();

        public float m_currentExplodingRadius => throw new System.NotImplementedException();

        public float m_currentExplodingDelay => throw new System.NotImplementedException();

        public float m_maxExplodingDelay => throw new System.NotImplementedException();

        public CircleCollider2D m_explosionCollider => throw new System.NotImplementedException();

        protected override void Update()
        {
            if (!m_isActive)
            {
                return;
            }

            if (m_lifetime > m_projectileData.maxLifetime)
            {
                m_parentPool.UnSpawn(gameObject);
            }

            m_lifetime += Time.deltaTime;

            GrenadeDirection();
        }

        public override void Shoot(Transform direction)
        {
            m_direction = new Vector2(direction.position.x, direction.position.y) + new Vector2(transform.position.x, transform.position.y);
            m_initialPosition = transform.position;
            m_centerPoint = (m_initialPosition + m_direction) / 2;
        }
        private void GrenadeDirection()
        {
            // Convert the 2D midpoint to 3D space, assuming z=0
            Vector3 rotationPoint = new Vector3(m_centerPoint.x, m_centerPoint.y, 0);

            // Rotate around the midpoint
            // Here, Vector3.forward is used as the rotation axis for 2D rotation
            // Modify the rotation speed if you want faster or slower rotation
            transform.RotateAround(rotationPoint, Vector3.forward, rotationSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, m_direction) < 0.2f)
            {
                m_lifetime = m_projectileData.maxLifetime;
            }
        }
        public void Explode()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableStart()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void StartExplosion()
        {
            throw new System.NotImplementedException();
        }
    }
}
