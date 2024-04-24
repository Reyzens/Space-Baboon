using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon
{
    public class BossSineProjectile : ShootingEnemyProjectile
    {
        //[SerializeField] private float m_sineWaveFrequency;
        //[SerializeField] private float m_sineWaveAmplitude;
        //
        //protected override void Move()
        //{
        //    m_direction = m_direction.normalized;
        //
        //    float horizontalMovement = Mathf.Sin(Time.time * m_sineWaveFrequency) * m_sineWaveAmplitude;
        //               
        //    Vector2 horizontalForce = m_direction * horizontalMovement;
        //               
        //    m_rb.AddForce(horizontalForce * m_projectileData.defaultAcceleration, ForceMode2D.Force);
        //
        //    if (m_direction.magnitude > 0)
        //        RegulateVelocity();
        //}
    }
}
