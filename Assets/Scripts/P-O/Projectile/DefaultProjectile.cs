using SpaceBaboon.WeaponSystem;
using UnityEngine;
namespace SpaceBaboon
{
    public class DefaultProjectile : Projectile
    {
        protected override void MovingDirection()
        {
            //Debug.Log("Called Default weapon MovingDirection with thos data : m_direction = " + m_direction + " m_projectileDataSpeed = " + m_projectileData.speed);
            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime, Space.World);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            m_parentPool.UnSpawn(gameObject);
        }
        public override void Shoot(ref Transform direction)
        {
            Vector2 newDirection = direction.position;
            Vector2 currentPosition = transform.position;
            m_direction = (newDirection - currentPosition).normalized;
            //Debug.Log("Called Default weapon shoot with those data : m_direction = " + m_direction);
        }
    }
}
