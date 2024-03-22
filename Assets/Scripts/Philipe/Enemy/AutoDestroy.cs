using UnityEngine;

namespace SpaceBaboon.Enemy
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float m_timer = 0.0f;

        void Start()
        {
            Destroy(gameObject, m_timer);
        }        
    }
}
