using System.Collections;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class FlameThrowerProjectile : Projectile
    {
        private Transform m_flamethrowerPosition;
        private ParticleSystem m_flames;
        private float m_flameRange;
        private float m_flameWidth;
        private float m_cooldownBetweenTick;
        [SerializeField] private float m_tickMaxDuration = 0.25f;
        [SerializeField] private float m_maxFlameDuration;
        private float m_currentFlameDuration;
        private bool m_isFiring;
        private PolygonCollider2D m_flameCollider;

        protected override void Awake()
        {
            m_flames = GetComponentInChildren<ParticleSystem>();
            m_flameCollider = GetComponent<PolygonCollider2D>();
        }
        public override void Shoot(Transform target, float maxRange, float attackZone, float damage, Transform weaponPosition = null)
        {
            base.Shoot(target, maxRange, attackZone, damage, weaponPosition);

            //The flame shoot from the weapon
            m_flamethrowerPosition = weaponPosition;
            m_target = target;

            //Values that scale from weapon
            m_flameRange = maxRange;
            m_flameWidth = attackZone;

            transform.localScale = new Vector3(m_flameRange, m_flameWidth);
            ParticleEffectInitialization();
        }
        protected override void Update()
        {
            if (m_flamethrowerPosition != null)
            {
                transform.position = m_flamethrowerPosition.position;

                Vector2 directionToTarget = m_target.position - transform.position;
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 100);
            }
            if (m_isFiring)
            {
                m_cooldownBetweenTick -= Time.deltaTime;
                m_currentFlameDuration -= Time.deltaTime;
                if (m_cooldownBetweenTick < 0)
                {
                    StartCoroutine(ColliderTickCoroutine());
                    m_cooldownBetweenTick = m_projectileData.speed;
                }
                if (m_currentFlameDuration < 0)
                {
                    m_parentPool.UnSpawn(gameObject);
                }
            }
        }

        private IEnumerator ColliderTickCoroutine()
        {
            m_flameCollider.enabled = true;
            float timePassed = 0;

            while (timePassed < m_tickMaxDuration)
            {
                timePassed += Time.deltaTime;
                yield return null;
            }

            m_flameCollider.enabled = false;
        }
        protected override void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            var emission = m_flames.emission;
            emission.enabled = value;
            if (!value)
            {
                m_flameCollider.enabled = value;
            }
            m_isFiring = value;
        }
        protected override void ResetValues(Vector2 pos)
        {
            m_lifetime = 0.0f;
            m_cooldownBetweenTick = -1;
            m_currentFlameDuration = m_maxFlameDuration;
        }
        protected void ParticleEffectInitialization()
        {
            m_flames.transform.localScale = new Vector3(m_flames.transform.localScale.x, transform.localScale.y, transform.localScale.x);
        }
    }
}
