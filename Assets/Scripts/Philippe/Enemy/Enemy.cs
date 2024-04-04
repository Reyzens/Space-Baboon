using SpaceBaboon.PoolingSystem;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class Enemy : Character, IPoolableGeneric
    {
        [SerializeField] private EnemyData m_enemyData;

        private GenericObjectPool m_parentPool;
        private bool m_isActive = false;

        private GameObject[] m_players; // TODO change how to get reference to player, maybe serialize the object

        private float m_health;
        private float m_bonusDamage = 0.0f;
        private float m_bonusAcceleration; // Pour simplifier on pourrait simplement avoir une acceleration de base qui ne change pas et un max Velocity qui peut changer
        private float m_bonusMaxVelocity;
        private float m_bonusAttackDelay;
        private float m_attackTimer = 0.0f;
        private bool m_attackReady = true;

        private void Awake()
        {
            m_characterRenderer = GetComponent<Renderer>();
            m_characterCollider = GetComponent<BoxCollider2D>(); // À changer pour circle éventuellement
            m_characterRb = GetComponent<Rigidbody2D>();
            m_health = m_enemyData.DefaultBaseHeatlh;
        }

        private void Start()
        {
            m_players = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            if (!m_isActive)
                return;

            if (!m_attackReady)
                ReadyAttack();
        }

        private void FixedUpdate()
        {
            if (!m_isActive)
                return;

            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                OnDamageTaken(collision.gameObject.GetComponent<WeaponSystem.Projectile>().OnHit());
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            SlightPushFromObstructingObject(collision);
        }

        private void SlightPushFromObstructingObject(Collision2D collision)
        {
            Vector3 direction = collision.transform.position - transform.position;
            m_characterRb.AddForce(-direction * m_enemyData.obstructionPushForce, ForceMode2D.Force);
        }

        private void ReadyAttack()
        {
            m_attackTimer -= Time.deltaTime;

            if (m_attackTimer < 0.0f)
                m_attackReady = true;
        }

        protected override void Move()
        {
            MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            Vector3 playerPosition = m_players[0].transform.position;

            Vector2 direction = (playerPosition - transform.position).normalized;
            m_characterRb.AddForce(direction * m_enemyData.DefaultBaseAcceleration /* + or * bonus */, ForceMode2D.Force);

            if (direction.magnitude > 0)
                RegulateVelocity();
        }

        protected override void RegulateVelocity()
        {
            if (m_characterRb.velocity.magnitude > m_enemyData.DefaultBaseMaxVelocity /* + or * bonus */)
            {
                m_characterRb.velocity = m_characterRb.velocity.normalized;
                m_characterRb.velocity *= m_enemyData.DefaultBaseMaxVelocity /* + or * bonus */;
            }
        }

        public void OnDamageTaken(float values)
        {
            m_health -= values;

            if (m_health <= 0)
                m_parentPool.UnSpawn(gameObject);
        }

        public float GetDamage() // TODO a discuter, fonctionnement de cette methode
        {
            if (!m_attackReady)
                return 0.0f;

            Attack();
            return m_enemyData.baseDamage /* + or * bonus */;
        }

        private void Attack()
        {
            m_attackTimer = m_enemyData.baseAttackDelay /* + or * bonus */;
            m_attackReady = false;
        }

        #region ObjectPooling
        public bool IsActive
        {
            get { return m_isActive; }
        }

        public void Activate(Vector2 pos, GenericObjectPool pool)
        {
            ResetValues(pos);
            SetComponents(true);
            m_parentPool = pool;
        }

        public void Deactivate()
        {
            SetComponents(false);
        }

        private void ResetValues(Vector2 pos)
        {
            transform.position = pos;
        }

        private void SetComponents(bool value)
        {
            m_isActive = value;
            m_characterRenderer.enabled = value;
            m_characterCollider.enabled = value;
        }
        #endregion
    }
}
