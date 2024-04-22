using SpaceBaboon.PoolingSystem;
using UnityEngine;

namespace SpaceBaboon.Crafting
{
    public class ResourceShards : MonoBehaviour, IPoolableGeneric
    {
        private Player m_recoltingPlayer;

        //For ObjectPool        
        protected GenericObjectPool m_parentPool;
        protected bool m_isActive = false;
        protected SpriteRenderer m_renderer;
        protected CircleCollider2D m_collider;

        // Update is called once per frame
        void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
            m_collider = GetComponent<CircleCollider2D>();
        }
        void Start()
        {

        }
        void Update()
        {

        }

        private void initialPush()
        {

        }

        public void Initialization(Vector2 direction, float pushStrenght, Player recoltingPlayer)
        {

        }

        #region ObjectPooling
        public bool IsActive
        {
            get { return m_isActive; }
        }

        public virtual void Activate(Vector2 pos, GenericObjectPool pool)
        {
            //Debug.Log("Activate parent grenade appeler");
            ResetValues(pos);
            SetComponents(true);

            m_parentPool = pool;
        }

        public virtual void Deactivate()
        {
            //Debug.Log("Deactivate parent grenade appeler");
            SetComponents(false);
        }
        protected virtual void ResetValues(Vector2 pos)
        {
            transform.position = pos;
        }
        protected virtual void SetComponents(bool value)
        {
            //Debug.Log("SetComponents parent appeler");
            m_isActive = value;
            m_renderer.enabled = value;
            m_collider.enabled = value;
        }
        #endregion

    }
}
