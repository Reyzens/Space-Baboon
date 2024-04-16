using UnityEngine;

namespace SpaceBaboon
{
    public class Character : BaseStats<MonoBehaviour>, SpaceBaboon.IDamageable, IStatsEditable
    {
        //BaseRef
        [SerializeField] protected CharacterData m_characterData;
        protected GameObject m_characterPrefab;
        protected SpriteRenderer m_renderer;
        protected BoxCollider2D m_collider;
        protected Rigidbody2D m_rB;
        
        //BaseVariables
        protected Vector2 m_movementDirection;
        protected float m_activeHealth;
        protected float m_activeVelocity;
        
        //BonusVariables
        protected float m_bonusHealth;
        protected float m_bonusVelocity;
        
        //Methods        
        protected virtual void Move(Vector2 values) {}

        protected virtual void RegulateVelocity() 
        {
            if (m_rB.velocity.magnitude > m_characterData.defaultMaxVelocity /* + or * bonus */)
            {
                m_rB.velocity = m_rB.velocity.normalized;
                m_rB.velocity *= m_characterData.defaultMaxVelocity /* + or * bonus */;
            }
        }

        protected virtual void CheckForSpriteDirectionSwap(Vector2 direction)
        {
            if (direction.x > 0)
            {
                m_renderer.flipX = false;
            }
            if (direction.x < 0)
            {
                m_renderer.flipX = true;
            }
        }   
        
        public virtual void OnDamageTaken(float values) {}

        public override ScriptableObject GetData() { return m_characterData; }
    }

}
