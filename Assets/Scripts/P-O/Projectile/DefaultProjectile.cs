using SpaceBaboon.WeaponSystem;
using UnityEngine;
namespace SpaceBaboon
{
    public class DefaultProjectile : Projectile
    {
        protected override void MovingDirection()
        {
            //Debug.Log("Called Default weapon MovingDirection with thos data : m_direction = " + m_direction + " m_projectileDataSpeed = " + m_projectileData.speed);
            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime);
        }

        public override void Shoot(Vector2 direction)
        {
            m_direction = direction.normalized;
            //Debug.Log("Called Default weapon shoot with those data : m_direction = " + m_direction);
        }
    }
}
