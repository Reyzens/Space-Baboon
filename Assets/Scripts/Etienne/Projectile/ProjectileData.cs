using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "NewProjectileData", menuName = "SpaceBaboon/ScriptableObjects/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
        public string projectileName;
        public float speed;
        public float damage;
        public float size;
        public float maxTravelDistance;
        public float maxLifetime;
    }
}
