using SpaceBaboon.PoolingSystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class Projectile : MonoBehaviour, IPoolableGeneric
    {
        [SerializeField] protected ProjectileData m_projectileData;

        protected Vector2 m_direction;
        protected Transform m_target;
        protected float m_lifetime = 0.0f;
        protected float m_bonusDmg = 0;
        protected float m_damage = 0;

        //For ObjectPool        
        protected GenericObjectPool m_parentPool;
        protected bool m_isActive = false;
        protected SpriteRenderer m_renderer;
        protected CircleCollider2D m_collider;




        protected virtual void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<CircleCollider2D>();
        }

        protected virtual void Start()
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
                Debug.Log("UnSpawning (lifetime)");
            }
            m_lifetime += Time.deltaTime;

            MovingDirection();
        }

        protected virtual void MovingDirection()
        {
            //Debug.Log("Called parent proj MovingDirection with thos data : m_direction = " + m_direction + " m_projectileDataSpeed = " + m_projectileData.speed);

            transform.Translate(m_direction * m_projectileData.speed * Time.deltaTime);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision) { }
        public virtual void Shoot(Transform direction, float maxRange, float attackZone, Transform playerPosition = null)
        {
            m_direction = new Vector2(direction.position.x, direction.position.y).normalized;
        }

        public virtual float OnHit()
        {
            return m_damage;
        }

        #region ObjectPooling
        public bool IsActive
        {
            get { return m_isActive; }
        }

        public virtual void Activate(Vector2 pos, GenericObjectPool pool)
        {
            //Debug.Log("Activate parent grenade appeler");
            ResetValues(pos);
            SetComponents(true);
            //m_isActive = true;

            m_parentPool = pool;
        }

        public virtual void Deactivate()
        {
            //Debug.Log("Deactivate parent grenade appeler");
            //m_isActive = false;
            SetComponents(false);
        }

        protected virtual void ResetValues(Vector2 pos)
        {
            m_lifetime = 0.0f;
            transform.position = pos;
        }
        protected virtual void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            m_renderer.enabled = value;
            m_collider.enabled = value;
        }
        #endregion


    }
}
