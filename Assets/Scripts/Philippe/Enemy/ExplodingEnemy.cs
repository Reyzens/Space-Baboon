using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class ExplodingEnemy : Enemy, IExplodable
    {
        [SerializeField] private GameObject m_projectilePrefab;
        [SerializeField] private Sprite m_chargingExplosionSprite;
        [SerializeField] private AnimationCurve m_colorChangeCurve;
        [SerializeField] private Color m_imminentExplosionColor;
        private Sprite m_baseSprite; // TODO maybe in scriptable object
        private float m_chargingExplosionTimer = 0.0f;
        private Color m_baseColor;        

        private Animator m_animator;

        private ExplodingEnemyData m_uniqueData;        

        private GameObject m_enemySpawner;
        private EnemySpawner m_enemySpawnerScript;

        private bool m_isChargingExplosion = false;
        
        protected override void Start()
        {
            base.Start();

            m_uniqueData = m_characterData as ExplodingEnemyData;
            
            // Maybe randomize distance to trigger bomb from data

            // TODO change this to ref to manager when GameManager is set up
            m_enemySpawner = GameObject.Find("EnemySpawner");
            m_enemySpawnerScript = m_enemySpawner.GetComponent<EnemySpawner>();

            m_baseSprite = m_renderer.sprite;
            m_baseColor = m_renderer.color;

            m_animator = GetComponent<Animator>();

            m_chargingExplosionTimer = m_uniqueData.delayBeforeExplosion;                                 
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            if (m_isChargingExplosion)
            {
                IExplodableUpdate();
                return;
            }

            if (m_distanceToPlayer < m_uniqueData.minDistanceForTriggeringBomb)
            {
                IExplodableSetUp();                
            }  
        }

        protected override void FixedUpdate()
        {
            if (!m_isActive)
                return;
        
            if (m_isChargingExplosion)
                return;

            Move(m_noVectorValue);
        }

        private void UpdateColorBasedOnAnimCurve()
        {
            float colorScale = m_colorChangeCurve.Evaluate(1 - (m_chargingExplosionTimer / m_uniqueData.delayBeforeExplosion));

            float r = Mathf.Lerp(m_baseColor.r, m_imminentExplosionColor.r, colorScale);
            float g = Mathf.Lerp(m_baseColor.g, m_imminentExplosionColor.g, colorScale);
            float b = Mathf.Lerp(m_baseColor.b, m_imminentExplosionColor.b, colorScale);

            Color newColor = m_renderer.color;
            newColor.r = r;
            newColor.g = g;
            newColor.b = b;
            m_renderer.color = newColor;
        }

        public void IExplodableSetUp()
        {
            m_isChargingExplosion = true;
            m_animator.enabled = false;
            m_rB.constraints = RigidbodyConstraints2D.FreezeAll;
            m_renderer.sprite = m_chargingExplosionSprite;            
        }

        public void IExplodableUpdate()
        {
            m_chargingExplosionTimer -= Time.deltaTime;

            UpdateColorBasedOnAnimCurve();

            if (m_chargingExplosionTimer < 0)
            {
                ResetVariables();
                Explode();
            }
        }

        public void Explode()
        {
            m_enemySpawnerScript.m_enemyProjectilesPool.Spawn(m_projectilePrefab, transform.position);
            m_parentPool.UnSpawn(gameObject);
        }

        public void StartExplosion() { /* Unused */ }        

        private void ResetVariables()
        {
            m_isChargingExplosion = false;
            m_animator.enabled = true;
            m_rB.constraints = RigidbodyConstraints2D.None;
            m_rB.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_renderer.sprite = m_baseSprite;
            m_renderer.color = m_baseColor;
            m_chargingExplosionTimer = m_uniqueData.delayBeforeExplosion;
        }

        public override bool CanAttack()
        {
            return false;
        }
    }
}
