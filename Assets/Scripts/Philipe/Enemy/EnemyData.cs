using UnityEngine;

namespace SpaceBaboon.Enemy
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
        public float baseMovementSpeed;
        public float baseAttackRange;
        public float baseAttackCooldown;
    }
}
