using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [RequireComponent(typeof(LineRenderer))]
    public class ShockWaveProjectile : Projectile, IExplodable
    {
        [SerializeField] protected int segments = 100;
        [SerializeField] protected float innerRadius = 5f;
        [SerializeField] protected float thickness = 0.5f;

        protected override void Start()
        {
            base.Start();
        }
        protected override void Update() { GenerateCircle(); }
        protected void GenerateCircle()
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();

            // Set the number of points
            lineRenderer.positionCount = segments;
            lineRenderer.useWorldSpace = false;

            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;

            float deltaTheta = (2f * Mathf.PI) / segments;
            float currentTheta = 0f;

            for (int i = 0; i < segments; i++)
            {
                float x = innerRadius * Mathf.Cos(currentTheta);
                float y = innerRadius * Mathf.Sin(currentTheta);
                Vector3 pos = new Vector3(x, y, 0);
                lineRenderer.SetPosition(i, pos);
                currentTheta += deltaTheta;
            }
            lineRenderer.loop = true;
        }
        public override void Shoot(ref Transform direction)
        {
            gameObject.transform.parent = direction;
        }
        public void Explode()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableSetUp()
        {
            throw new System.NotImplementedException();
        }

        public void IExplodableUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void StartExplosion()
        {
            throw new System.NotImplementedException();
        }
    }
}
