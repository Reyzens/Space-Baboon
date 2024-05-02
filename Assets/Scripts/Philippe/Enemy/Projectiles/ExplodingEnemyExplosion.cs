using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ExplodingEnemyExplosion : Projectile
    {
        private ExplodingEnemyProjectileData m_uniqueData;
        private Vector3 m_initialScale;

        private Animator m_animator; //May be needed for explosion animation
                
        protected void Start()
        {
            VariablesSetUp();            
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
            float curveValue = m_uniqueData.explosionSizeScalingCurve.Evaluate(m_lifetime);
            
            float newScale = Mathf.Lerp(m_initialScale.x, m_uniqueData.maxExplosionSize, curveValue);
            transform.localScale = new Vector3(newScale, newScale, 0);
        }

        private void UpdateDamageBasedOnAnimCurve()
        {
            // TODO maybe
        }

        private void VariablesSetUp()
        {
            m_uniqueData = m_projectileData as ExplodingEnemyProjectileData;
            m_animator = GetComponent<Animator>();
            m_initialScale = transform.localScale;
            m_damage = m_uniqueData.damage;
        }

        public override float OnHit()
        {
            //Debug.Log("OnHit called by :  " + gameObject.name + "with " + m_uniqueData.damage + " damage");
            return m_uniqueData.damage;
        }               
    }
}
