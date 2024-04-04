using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "SpaceBaboon/ScriptableObjects/EnemyData")]
    public class EnemyData : CharacterData
    {
        [Header("EnemyUniqueStats")]        
        public int baseDamage;        
        public float size; //TODO maybe remove            
        public float baseAttackRange; //TODO maybe remove
        public float baseAttackDelay;
        public float obstructionPushForce;

        // TODO enemy data prefab may be obsolete
    }
}
