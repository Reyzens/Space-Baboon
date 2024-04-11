using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ExplodingEnemyProjectile : Projectile, IExplodable
    {
        [SerializeField] private ExplodableData m_explodableData;
        private ExplodingEnemyProjectileData m_uniqueData;

        private Player m_player;

        [SerializeField] private AnimationCurve m_colorChangeCurve;
        [SerializeField] private Color m_imminentExplosionColor;
        private float m_colorChangeTimer = 0.0f;
        private Color m_baseColor;
        private SpriteRenderer m_spriteRenderer;

        private float m_explosionTimer = 0.0f;
        private bool m_isExploding = false;
        private float m_currentExplosionTime = 0.0f;        
        private Vector3 m_initialScaleOfProjectile;

        protected override void Start()
        {
            base.Start();

            m_uniqueData = m_projectileData as ExplodingEnemyProjectileData;

            m_explosionTimer = m_uniqueData.delayBeforeExplosion;
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_baseColor = m_spriteRenderer.color;
            
            //m_rb = GetComponent<Rigidbody2D>();

            // TODO to change find, most likely a reference that would be stored in an upcoming gameManager                  
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            IExplodableSetUp();
        }

        protected override void Update()
        {            
            if (!m_isActive)
            {
                return;
            }

            if (m_isExploding)
            {
                IExplodableUpdate();
                return;
            }

            if (m_explosionTimer < 0.0f)
            {                
                StartExplosion();
                return;
            }

            m_explosionTimer -= Time.deltaTime;
            
            UpdateColorBasedOnAnimCurve();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && m_isExploding)
            {
                m_player.OnDamageTaken(m_projectileData.damage);
                m_collider.enabled = false;
            }
        }

        private void UpdateColorBasedOnAnimCurve()
        {
            m_colorChangeTimer += Time.deltaTime;

            float colorScale = m_colorChangeCurve.Evaluate(m_colorChangeTimer / m_uniqueData.delayBeforeExplosion);
            
            float r = Mathf.Lerp(m_baseColor.r, m_imminentExplosionColor.r, colorScale);
            float g = Mathf.Lerp(m_baseColor.g, m_imminentExplosionColor.g, colorScale);
            float b = Mathf.Lerp(m_baseColor.b, m_imminentExplosionColor.b, colorScale);
                        
            Color newColor = m_spriteRenderer.color;
            newColor.r = r;
            newColor.g = g;
            newColor.b = b;
            m_spriteRenderer.color = newColor;            
        }        

        public void IExplodableSetUp()
        {
            // May become obsolete after explosion fx integration
            m_initialScaleOfProjectile = transform.localScale;
        }

        public void IExplodableUpdate()
        {
            if (m_currentExplosionTime < 0)
            {
                m_parentPool.UnSpawn(gameObject);
            }

            if (m_isExploding)
            {
                m_currentExplosionTime -= Time.deltaTime;
            }
        }

        public void StartExplosion()
        {
            m_isExploding = true;
            m_currentExplosionTime = m_explodableData.m_maxExplosionTime;
            Explode();
        }

        public void Explode()
        {
            // May become obsolete after explosion fx integration
            gameObject.transform.localScale = new Vector2(m_explodableData.m_explosionRadius, m_explodableData.m_explosionRadius);
            // Maybe use coroutine for explosion scale expansion
        }

        protected override void ResetValues(Vector2 pos)
        {
            //Debug.Log("current Explosion time" + );

            m_explosionTimer = m_uniqueData.delayBeforeExplosion;
            Debug.Log("m_uniqueData.delayBeforeExplosion " + m_uniqueData.delayBeforeExplosion);
            m_currentExplosionTime = 0.0f;
            m_isExploding = false;
            m_colorChangeTimer = 0.0f;
            m_spriteRenderer.color = m_baseColor;
            m_lifetime = m_uniqueData.maxLifetime;
            transform.position = pos;
            gameObject.transform.localScale = m_initialScaleOfProjectile;
        }
    }
}
