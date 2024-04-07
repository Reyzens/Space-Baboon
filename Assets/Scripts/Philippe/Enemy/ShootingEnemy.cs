using UnityEngine;
//using SpaceBaboon.WeaponSystem;

namespace SpaceBaboon.EnemySystem
{
    public class ShootingEnemy : Enemy
    {
        [SerializeField] private ShootingEnemyData m_uniqueData;
        //[SerializeField] private ShootingEnemyWeapon m_weapon;

        private float m_distanceToPlayer;
        
        protected override void Update()
        {
            base.Update();
            CalculateDistanceToPlayer();           
        }

        protected override void Move(Vector2 value)
        {
            if(m_distanceToPlayer > m_uniqueData.maxTargetAcquiringRange)
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
