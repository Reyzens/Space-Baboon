using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "NewExplodingEnemyProjectileData", menuName = "SpaceBaboon/ScriptableObjects/ExplodingEnemyProjectileData")]
    public class ExplodingEnemyProjectileData : ProjectileData
    {
        [Header("ExplodingEnemyProjectileUniqueStats")]
        public int test;
        //public float delayBeforeExplosion;
    }
}
