using System.Collections;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class PlayerSwordSwing : Projectile, IPiercing
    {
        [SerializeField] private Transform m_hiltPosition;
        private float m_swingArc;
        private Transform m_middleSwingDirection;
        public override void Shoot(ref Transform direction, float maxRange, float attackZone)
        {
            m_middleSwingDirection = direction;
            m_swingArc = attackZone * 45;
            StartSwing();
        }
        protected override void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            m_renderer.enabled = value;
            //m_collider.enabled = value;
        }
        private void RotateArounbHilt()
        {
            transform.RotateAround(m_hiltPosition.position, Vector3.forward, m_swingArc);
        }
        protected void StartSwing()
        {
            StartCoroutine(SwingRoutine());
        }
        private IEnumerator SwingRoutine()
        {
            float timeSwinging = 0.0f;

            while (timeSwinging < m_projectileData.speed)
            {
                float currentRotation = Mathf.Lerp(0, m_swingArc, timeSwinging / m_projectileData.speed);

                transform.RotateAround(m_hiltPosition.position, Vector3.forward, currentRotation - transform.eulerAngles.z);

                timeSwinging += Time.deltaTime;
                yield return null;
            }
        }
        public void IPiercingSetUp()
        {
            throw new System.NotImplementedException();
        }

        public void LastPierce()
        {
            throw new System.NotImplementedException();
        }

        public void OnPiercing()
        {
            throw new System.NotImplementedException();
        }
    }
}
