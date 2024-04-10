using System.Collections;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class PlayerSwordSwing : Projectile, IPiercing
    {
        [SerializeField] private float m_attackZoneScaling;
        private Transform m_playerPos;
        private float m_swingArc;

        public override void Shoot(Transform direction, float maxRange, float attackZone, Transform swordPosition)
        {
            m_playerPos = swordPosition;
            StartSwing(direction, maxRange, attackZone);
        }

        protected void StartSwing(Transform direction, float maxRange, float attackZone)
        {
            m_swingArc = m_attackZoneScaling * attackZone;
            StartCoroutine(SwingCoroutine(direction, maxRange, attackZone));
        }
        private IEnumerator SwingCoroutine(Transform direction, float maxRange, float attackZone)
        {
            //Vector2 targetDirection = (direction.position - transform.position).normalized;

            Vector2 directionVector = direction.position - transform.position;
            float directionAngle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            float startAngle = directionAngle - m_swingArc / 2 - 90;
            float endAngle = directionAngle + m_swingArc / 2 - 90;

            float currentTime = 0.0f;
            float t;
            float currentAngle;

            while (currentTime < m_projectileData.speed)
            {
                currentTime += Time.deltaTime;
                t = currentTime / m_projectileData.speed;

                currentAngle = Mathf.Lerp(startAngle, endAngle, t);

                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                transform.position = m_playerPos.position;

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

        protected override void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            m_renderer.enabled = value;
            //m_collider.enabled = value;
        }
    }
}

//public override void Shoot(ref Transform direction, float maxRange, float attackZone)
//{
//    m_middleSwingDirection = direction;
//    m_swingArc = attackZone * m_attackZoneScaling;
//    StartSwing();
//}
//protected override void SetComponents(bool value)
//{
//    //Debug.Log("SetComponents parent appeler");
//    m_isActive = value;
//    m_renderer.enabled = value;
//    //m_collider.enabled = value;
//}
////protected void StartSwing()
////{
////    StartCoroutine(SwingRoutine());
////}
////private IEnumerator SwingRoutine()
////{
////    ////float timeSwinging = 0.0f;

////    //float timeSwinging = 0.0f;
////    //float swingDuration = m_projectileData.speed; // Assuming this represents the duration of the swing

////    //Vector2 middleVector = m_middleSwingDirection.position - transform.position;

////    //float startRotation = m_middleSwingDirection.eulerAngles.z - m_swingArc / 2;
////    //float endRotation = m_middleSwingDirection.eulerAngles.z + m_swingArc / 2;

////    //while (timeSwinging < swingDuration)
////    //{
////    //    //float currentRotation = Mathf.Lerp(0, m_swingArc, timeSwinging / m_projectileData.speed);

////    //    //transform.RotateAround(m_hiltPosition.position, Vector3.forward, currentRotation - transform.eulerAngles.z);

////    //    //timeSwinging += Time.deltaTime;
////    //    //yield return null;

////    //    float currentRotation = Mathf.Lerp(startRotation, endRotation, timeSwinging / swingDuration);

////    //    transform.rotation = Quaternion.Euler(0, 0, currentRotation);

////    //    timeSwinging += Time.deltaTime;
////    //    yield return null;
////    //}
////}