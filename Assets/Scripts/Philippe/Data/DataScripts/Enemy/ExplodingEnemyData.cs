using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewExplodingEnemyData", menuName = "SpaceBaboon/ScriptableObjects/ExplodingEnemyData")]
    public class ExplodingEnemyData : EnemyData
    {
        [Header("ExplodingEnemyUniqueStats")]
        public float minDistanceForTriggeringBomb;
        public float delayBeforeExplosion;
    }
}
