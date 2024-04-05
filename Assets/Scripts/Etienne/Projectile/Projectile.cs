using SpaceBaboon.PoolingSystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] protected ProjectileData m_projectileData;

        protected Vector2 m_direction;
        protected float m_lifetime = 0.0f;
        protected float m_bonusDmg = 0;
        protected float m_damage = 0;

        //For ObjectPool
        protected bool m_isActive = false;
        protected SpriteRenderer m_renderer;
        protected CircleCollider2D m_collider;
        protected ObjectPool m_parentPool;

        public bool IsActive
        {
            get { return m_isActive; }
        }

        protected void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<CircleCollider2D>();
        }

        protected void Start()
        {
            m_damage = m_projectileData.damage;
        }

        protected virtual void Update()
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

            MovingDirection();
        }

        protected virtual void MovingDirection()
        {
            //Debug.Log("Called parent proj MovingDirection with thos data : m_direction = " + m_direction + " m_projectileDataSpeed = " + m_projectileData.speed);

            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            m_parentPool.UnSpawn(gameObject);
            //Debug.Log("projectile hit: " + collision.gameObject.name);
        }

        public virtual void Shoot(Vector2 direction)
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

        protected void ResetValues(Vector2 pos)
        {
            m_lifetime = 0.0f;
            transform.position = pos;
        }
        protected void SetComponents(bool value)
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
