using UnityEngine;

namespace SpaceBaboon
{
    public class TimedAutoDestroy : MonoBehaviour
    {
        [SerializeField] private float m_timer = 0.0f;

        void Start()
        {
            Destroy(gameObject, m_timer);
        }        
    }
}
