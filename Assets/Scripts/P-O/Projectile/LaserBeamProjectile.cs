using System;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class LaserBeamProjectile : Projectile, IPiercing
    {
        //IPiercing variables
        [SerializeField] PiercingData m_PiercingData;
        private int m_currentPiercingLeft;
        private Transform m_lastEnemyPosition;

        protected override void Start()
        {
            base.Start();
        }
        public override void Shoot(ref Transform direction)
        {
            int enemyHit = 0;
            RaycastHit2D[] enemyHits = Physics2D.RaycastAll(transform.position, direction.position);

            foreach (RaycastHit2D hit in enemyHits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    enemyHit++;
                    if (enemyHit == m_PiercingData.m_maxAmountOfPiercing)
                    {
                        m_lastEnemyPosition = hit.collider.transform;
                    }
                }
            }
        }
        public void IPiercingSetUp()
        {
            m_currentPiercingLeft = m_PiercingData.m_maxAmountOfPiercing;
        }

        public void LastPierce()
        {
            throw new NotImplementedException();
        }

        public void OnPiercing()
        {
            throw new NotImplementedException();
        }
    }
}
