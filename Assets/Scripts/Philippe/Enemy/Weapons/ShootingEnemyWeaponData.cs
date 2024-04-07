using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "SpaceBaboon/ScriptableObjects/ShootingEnemyWeaponData")]
    public class ShootingEnemyWeaponData : WeaponData
    {
        // Peut-etre a delete

        [Header("ShootingEnemyWeaponUniqueStats")]
        public int test;
    }
}
