using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ExplodingEnemyExplosion : Projectile
    {
        // TODO Check if I need to integrate IExplodableData scriptable object,
        // right now the exploding enemy logic don't need it. Delete scriptable object 
        // in project if I realise I don't use it
        private ExplodingEnemyProjectileData m_uniqueData;
        [SerializeField] private AnimationCurve m_sizeChangeCurve;       

        private Player m_player;        
        private Animator m_animator;

        private Vector3 m_initialScale;

        protected void Start()
        {
            m_uniqueData = m_projectileData as ExplodingEnemyProjectileData;       

            // TODO to change find, most likely a reference that would be stored in an upcoming gameManager                  
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            m_animator = GetComponent<Animator>();

            m_initialScale = transform.localScale;

            m_damage = m_uniqueData.damage;
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;
            
            UpdateScaleBasedOnAnimCurve();            
        }

        private void UpdateScaleBasedOnAnimCurve()
        {            
            float curveValue = m_sizeChangeCurve.Evaluate(m_lifetime);
            
            float newScale = Mathf.Lerp(m_initialScale.x, m_uniqueData.maxExplosionSize, curveValue);
            transform.localScale = new Vector3(newScale, newScale, 0);
        }

        private void UpdateDamageBasedOnAnimCurve()
        {
            // TODO maybe
        }

        public override float OnHit()
        {
            Debug.Log("OnHit called by :  " + gameObject.name + "with " + m_uniqueData.damage + " damage");
            return m_uniqueData.damage;
        }

        //protected override void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Player") /*&& m_isExploding*/)
        //    {
        //        m_player.OnDamageTaken(m_projectileData.damage);
        //        m_collider.enabled = false;
        //    }
        //}        
    }
}
