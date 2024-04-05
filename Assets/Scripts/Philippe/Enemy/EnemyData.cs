using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "SpaceBaboon/ScriptableObjects/EnemyData")]
    public class EnemyData : CharacterData
    {
        [Header("EnemyUniqueStats")]        
        public int defaultContactAttackDamage;
        public float defaultContactAttackDelay;
        public float defaultAttackRange; //TODO maybe remove        
        public float size; //TODO maybe remove
        public float obstructionPushForce;        
    }
}
