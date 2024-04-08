using UnityEngine;
using SpaceBaboon.WeaponSystem;

namespace SpaceBaboon.EnemySystem
{
    public class ShootingEnemy : Enemy
    {
        [SerializeField] private ShootingEnemyData m_uniqueData;
        private ShootingEnemyWeapon m_weapon;

        private float m_distanceToPlayer;
        private float m_targetAcquisitionTimer = 0.0f;
        private bool m_targetInRange = false;

        protected override void Start()
        {
            base.Start();
            m_weapon = GetComponentInChildren<ShootingEnemyWeapon>();
            m_targetAcquisitionTimer = m_uniqueData.targetAcquisitionDelay;
        }

        protected override void Update()
        {
            base.Update();
            
            if (!m_isActive)
                return;
            
            CalculateDistanceToPlayer();

            if (m_distanceToPlayer > m_uniqueData.maxTargetAcquisitionRange)
                return;

            TryToAcquireTarget(); // TODO check the logic, may be possible to simplify
        }

        private void TryToAcquireTarget()
        {
            if (m_targetAcquisitionTimer < 0.0f && m_contactAttackReady)
            {
                AimWeapon();
                return;
            }

            if (m_distanceToPlayer < m_uniqueData.maxTargetAcquisitionRange)
            {
                m_targetAcquisitionTimer -= Time.deltaTime;
            }            
        }
        
        private void AimWeapon()
        {            
            m_targetAcquisitionTimer = m_uniqueData.targetAcquisitionDelay;
            m_weapon.GetTarget(m_player.transform);
        }

        protected override void Move(Vector2 value)
        {
            // TODO see if I can bring improvements
            if(m_distanceToPlayer > m_uniqueData.maxTargetAcquisitionRange)
            {
                base.Move(value);
                return;
            }

            if (m_rB.velocity.magnitude < 0.1f)
            {
                m_rB.velocity = Vector2.zero;
                return;
            }                

            StopMovement();
        }

        private void CalculateDistanceToPlayer()
        {            
            m_distanceToPlayer = Vector3.Distance(transform.position, m_player.transform.position);
        }

        private void StopMovement()
        {
            m_rB.AddForce(-m_rB.velocity.normalized * m_data.defaultAcceleration, ForceMode2D.Force);
        }
    }
}
