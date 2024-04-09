using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
    public class LaserBeamProjectile : Projectile, IPiercing
    {
        private LineRenderer m_laserDisplay;
        private EdgeCollider2D m_laserHitBox;
        private Transform m_initialWeaponPosition;
        [SerializeField] private float m_laserWidth;
        private float m_currentLaserDuration;
        [SerializeField] private float m_maxLaserDuration;
        //IPiercing variables
        [SerializeField] PiercingData m_PiercingData;
        private int m_currentPiercingLeft;
        private Transform m_lastEnemyPosition;

        protected override void Start()
        {
            base.Start();
            IPiercingSetUp();
        }
        public override void Shoot(ref Transform direction)
        {
            //Debug.Log("Laser was shot");
            int enemyHit = 0;
            RaycastHit2D[] enemyHits = Physics2D.RaycastAll(transform.position, direction.position - transform.position);

            foreach (RaycastHit2D hit in enemyHits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    enemyHit++;
                    m_lastEnemyPosition = hit.collider.transform;
                    if (enemyHit == m_PiercingData.m_maxAmountOfPiercing)
                    {
                        //Debug.Log("Laser raycast hit an enemy");
                        OnPiercing();
                        return;
                    }
                }
            }
            SetLaserPosition(m_initialWeaponPosition.position, m_lastEnemyPosition.position);
        }
        protected override void MovingDirection()
        {
        }
        public void IPiercingSetUp()
        {
            m_currentPiercingLeft = m_PiercingData.m_maxAmountOfPiercing;
            m_laserDisplay = GetComponent<LineRenderer>();
            m_laserHitBox = GetComponent<EdgeCollider2D>();
            m_laserDisplay.startWidth = m_laserWidth;
            m_laserDisplay.endWidth = m_laserWidth;
        }
        protected override void Update()
        {
            base.Update();
            OnPiercing();
        }
        public void LastPierce()
        {
            throw new NotImplementedException();
        }

        public void OnPiercing()
        {

        }

        protected override void ResetValues(Vector2 pos)
        {
            base.ResetValues(pos);
            m_currentLaserDuration = m_maxLaserDuration;
        }
        protected override void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            //m_laserDisplay.enabled = value;
        }
        private void SetLaserPosition(Vector2 startPositon, Vector2 endPosition)
        {
            m_laserDisplay.positionCount = 2;
            m_laserDisplay.SetPosition(0, m_initialWeaponPosition.position);
            m_laserDisplay.SetPosition(1, m_lastEnemyPosition.position);

            m_laserHitBox.SetPoints(new List<Vector2> { startPositon, endPosition });
        }
    }
}
