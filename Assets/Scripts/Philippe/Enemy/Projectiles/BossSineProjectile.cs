using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class BossSineProjectile : ShootingEnemyProjectile
    {
        // TODO maybe put it in a special scriptable object data
        [SerializeField] private float m_sineWaveFrequency;
        [SerializeField] private float m_sineWaveAmplitude;                

        protected override void Move()
        {
            //TODO maybe change to an anim curve
            float horizontalMovement = Mathf.Sin(Time.time * m_sineWaveFrequency) * m_sineWaveAmplitude;                        
            Vector2 perpendicularDirection = new Vector2(-m_direction.y, m_direction.x);
            Vector2 combinedDirection = m_direction + perpendicularDirection * horizontalMovement;

            m_rb.AddForce(combinedDirection.normalized * m_projectileData.defaultAcceleration, ForceMode2D.Force);

            if (combinedDirection.magnitude > 0)
                RegulateVelocity();
        }
    }
}
