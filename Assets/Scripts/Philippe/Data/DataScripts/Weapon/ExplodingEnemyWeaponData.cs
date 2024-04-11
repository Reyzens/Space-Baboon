using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [CreateAssetMenu(fileName = "NewExplodingEnemyWeaponData", menuName = "SpaceBaboon/ScriptableObjects/ExplodingEnemyWeaponData")]
    public class ExplodingEnemyWeaponData : WeaponData
    {
        [Header("ExplodingEnemyWeaponUniqueStats")]
        public int test;
    }
}
