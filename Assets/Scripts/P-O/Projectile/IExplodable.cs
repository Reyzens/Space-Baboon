using UnityEngine;

namespace SpaceBaboon
{
    public interface IExplodable
    {
        float m_currentExplodingTimer { get; }
        float m_maxExplodingTime { get; }
        float m_maxExplodingRadius { get; }
        float m_currentExplodingRadius { get; }
        float m_currentExplodingDelay { get; }
        float m_maxExplodingDelay { get; }
        CircleCollider2D m_explosionCollider { get; }
        void StartExplosion();
        void Explode();
        void IExplodableUpdate();
        void IExplodableStart();
    }
}
