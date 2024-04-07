using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "SpaceBaboon/ScriptableObjects/ShootingEnemyData")]
    public class ShootingEnemyData : ScriptableObject
    {
        [Header("ShootingEnemyUniqueStats")]
        public float maxTargetAcquiringRange;



    }
}
