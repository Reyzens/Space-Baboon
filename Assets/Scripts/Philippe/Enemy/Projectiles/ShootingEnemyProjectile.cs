using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ShootingEnemyProjectile : Projectile
    {
        private GameObject m_playerObject;
        protected Player m_player;        
        private CircleCollider2D m_triggerCollider;

        protected override void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();            
            m_rb = GetComponent<Rigidbody2D>();

            CircleCollider2D[] colliders = GetComponents<CircleCollider2D>();
            foreach (CircleCollider2D collider in colliders)
            {
                if (collider.isTrigger)
                    m_triggerCollider = collider;
                else
                    m_collider = collider;
            }
        }

        protected void Start()
        {
            m_playerObject = GameObject.FindGameObjectWithTag("Player"); // TODO to change, most likely a reference that would be stored in an upcoming gameManager           
            m_player = m_playerObject.GetComponent<Player>();
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            Move();
        }

        public override void Shoot(Transform direction, float maxRange, float attackZone, float damage, Transform playerPosition)
        {
            Vector2 newDirection = direction.position;
            Vector2 currentPosition = transform.position;
            m_direction = (newDirection - currentPosition).normalized;
            m_damage = damage;
        }

        public override float OnHit()
        {
            m_parentPool.UnSpawn(gameObject);
            return base.OnHit();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {           
            if (m_parentPool != null)
                m_parentPool.UnSpawn(gameObject);
        }

        protected override void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            m_renderer.enabled = value;
            m_collider.enabled = value;
            m_triggerCollider.enabled = value;
        }
    }
}
