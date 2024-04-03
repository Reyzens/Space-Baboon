using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class Character : MonoBehaviour  , SpaceBaboon.IDamageable
    {
        //BaseRef
        protected CharacterData m_characterData;
        protected GameObject m_characterPrefab;
        protected Renderer m_characterRenderer;
        protected BoxCollider2D m_characterCollider;
        protected Rigidbody2D m_characterRb;
        
        //BaseVariables
        protected float m_currentHealth;
        protected float m_currentMovespeed;
        protected float m_currentAcceleration;
        protected float m_currentVelocity;
        
        //BonusVariables
        protected float m_bonusHealth;
        protected float m_bonusMovespeed;
        
        //Methods
        protected virtual void Move() {} 
        protected virtual void Move(Vector2 values) {}

        protected virtual void RegulateVelocity() {}
        
        public virtual void OnDamageTaken(float values) {}
    }
}
