using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private ProjectileData m_projectileData;

        private Vector2 m_direction;
        private float m_lifetime = 0.0f;
        private bool m_isActive = false;
        private float m_bonusDmg = 0;
        private float m_damage = 0;

        

        SpriteRenderer m_renderer;
        CircleCollider2D m_collider;

        ObjectPool m_parentPool;

        public bool IsActive
        {
            get { return m_isActive; }
            //set { m_isActive = value; } // private set ?
        }

        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            m_damage = m_projectileData.damage;
        }

        private void Update()
        {
            if (!m_isActive)
            {
                return;
            }

            if (m_lifetime > m_projectileData.maxLifetime)
            {
                m_parentPool.UnSpawn(gameObject);
                //Debug.Log("UnSpawning (lifetime)");

            }
            m_lifetime += Time.deltaTime;

            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            m_parentPool.UnSpawn(gameObject);
            //Debug.Log("projectile hit: " + collision.gameObject.name);
        }

        public void Shoot(Vector2 direction)
        {
            m_direction = direction.normalized;
        }

        public void Activate(Vector2 pos, ObjectPool pool)
        {
            ResetValues(pos);
            SetComponents(true);
            m_isActive = true;

            m_parentPool = pool;
        }


        public void Deactivate()
        {
            m_isActive = false;
            SetComponents(false);
        }

        private void ResetValues(Vector2 pos)
        {
            m_lifetime = 0.0f;
            transform.position = pos;
        }
        private void SetComponents(bool value)
        {
            m_renderer.enabled = value;
            m_collider.enabled = value;
        }

        public float GetDamage()
        {
            return m_damage;
        }
    }
}
