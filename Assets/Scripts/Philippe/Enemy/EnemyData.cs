using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "SpaceBaboon/ScriptableObjects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public string name;
        public Sprite sprite;
        public GameObject prefab;
        public int baseHealth;
        public int baseDamage;        
        public float size;
        public float baseAcceleration;
        public float baseMaxVelocity;        
        public float baseAttackRange;
        public float baseAttackDelay;
        public float obstructionPushForce;
    }
}
