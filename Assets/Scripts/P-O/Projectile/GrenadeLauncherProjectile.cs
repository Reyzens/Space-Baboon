using SpaceBaboon.WeaponSystem;
using System.Collections;
using UnityEngine;

namespace SpaceBaboon
{
    public class GrenadeLauncherProjectile : Projectile, IExplodable
    {
        //Private variables
        [SerializeField] private AnimationCurve m_grenadeCurve;
        [SerializeField] private float m_curveMaxHeight;
        [SerializeField] private float m_curveDuration;
        private Vector2 m_lastTargetPosition;
        private Vector2 m_initialShootingPosition;
        private float m_initialDistanceToTarget;
        public float m_currentExplodingTimer => throw new System.NotImplementedException();

        public float m_maxExplodingTime => throw new System.NotImplementedException();

        public float m_maxExplodingRadius => throw new System.NotImplementedException();

        public float m_currentExplodingRadius => throw new System.NotImplementedException();

        public float m_currentExplodingDelay => throw new System.NotImplementedException();

        public float m_maxExplodingDelay => throw new System.NotImplementedException();

        public CircleCollider2D m_explosionCollider => throw new System.NotImplementedException();

        protected override void Start()
        {
            base.Start();
            m_target = null;
        }
        public override void Shoot(ref Transform direction)
        {
            m_target = direction;
            m_initialShootingPosition = transform.position;
            Vector2 initialPosition = transform.position;
            if (m_target != null)
            {
                Debug.Log(m_target.position);
                m_initialDistanceToTarget = Vector2.Distance(m_target.position, initialPosition);
                m_lastTargetPosition = m_target.position;
            }
            StartCoroutine(Curve(transform.position, m_target.position));
        }

        public IEnumerator Curve(Vector2 start, Vector2 end)
        {
            float timePassed = 0.0f;

            if (m_target != null)
            {
                while (timePassed < m_curveDuration)
                {
                    timePassed += Time.deltaTime;

                    float timeSpeedScaling = timePassed / m_curveDuration;
                    float heightScaling = m_grenadeCurve.Evaluate(timeSpeedScaling);

                    float directionHeightModifier = Mathf.Lerp(0.0f, m_curveMaxHeight, heightScaling);

                    transform.position = Vector2.Lerp(start, end, timeSpeedScaling) + new Vector2(0.0f, directionHeightModifier);

                    yield return null;
                }
            }
        }
        public void Explode()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableStart()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void StartExplosion()
        {
            throw new System.NotImplementedException();
        }
    }
}
