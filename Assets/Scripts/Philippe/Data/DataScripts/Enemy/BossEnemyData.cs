using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewBossEnemyData", menuName = "SpaceBaboon/ScriptableObjects/BossEnemyData")]
    public class BossEnemyData : EnemyData
    {
        [Header("EnemyUniqueStats")]
        public int test;
        public List<Sprite> sprites;
        public float possibleAggroRange;
        public float playerAggroRange;
        public float craftingStationAttackRange;
        public float craftingStationAttackDelay;
        public float specialAttackDelay;
        public float specialAttackChargeDelay;        
        public float basicAttackDelay;
        public int basicAttacksBeforeSpecial;
    }
}
