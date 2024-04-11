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
        private bool m_isFiring;
        private PolygonCollider2D m_flameCollider;

        protected override void Awake()
        {
            //base.Awake();
            m_flames = GetComponentInChildren<ParticleSystem>();
            m_flameCollider = GetComponent<PolygonCollider2D>();
        }
        public override void Shoot(Transform direction, float maxRange, float attackZone, Transform weaponPosition = null)
        {
            m_flamethrowerPosition = weaponPosition;
            m_target = direction;
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

                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.transform.position.z - transform.position.z;
                Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                //m_target.position = cursorWorldPosition;

                Vector2 directionToCursor = cursorWorldPosition - transform.position;
                float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 200); // Adjust rotation speed as necessary
            }
            if (m_isFiring)
            {
                m_cooldownBetweenTick -= Time.deltaTime;

                if (m_cooldownBetweenTick < 0)
                {
                    StartCoroutine(ColliderTickCoroutine());
                    m_cooldownBetweenTick = m_projectileData.speed;
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
            //m_renderer.enabled = value;
            //m_collider.enabled = value;
        }
        protected override void ResetValues(Vector2 pos)
        {
            //base.ResetValues(pos);
            m_lifetime = 0.0f;
            m_cooldownBetweenTick = -1;
            m_isFiring = true;
        }
        protected void ParticleEffectInitialization()
        {
            m_flames.transform.localScale = new Vector3(m_flames.transform.localScale.x, transform.localScale.y, transform.localScale.x);
        }
    }
}
