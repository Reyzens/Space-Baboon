using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "SpaceBaboon/ScriptableObjects/ShootingEnemyProjectileData")]
    public class ShootingEnemyProjectileData : ProjectileData
    {
        [Header("ShootingEnemyProjectileUniqueStats")]
        public int test;
    }
}
